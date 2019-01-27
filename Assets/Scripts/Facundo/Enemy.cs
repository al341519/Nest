using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    float health = 25;
    int direction = -1;
    float speed =2;
    float sizeX, sizeY;
    float range = 10;
    float minDistance = 1;
    float enemyWatchedTimer = 0;
    bool watched = false;
    public int damage;
    Transform transformPlayer;
    Coroutine attack;

    bool stay = false;


    float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        sizeX = this.gameObject.GetComponent<Collider>().bounds.size.x/2;
        sizeY = this.gameObject.GetComponent<Collider>().bounds.size.y/2;

    }

    // Update is called once per frame
    void Update()
    {

        if (!watched)
        {
            patrol();
            print("PATROL");
        }

        if (watched)
        {
            Chase();
            print("CHASE");

            enemyWatchedTimer += Time.deltaTime;
            print(enemyWatchedTimer);
            if (enemyWatchedTimer >= 3)
            {
                watched = false;
                stay = false;
                enemyWatchedTimer = 0;
            }
        }
        if (attack != null)
        {
            attackTimer += Time.deltaTime;
        }
    }
    private void patrol()
    {

        if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + 0.2f)&&!stay)
        {
            stay = true;
            StartCoroutine(turn(0.8f));
        }
        RaycastHit hit1;
        bool hited1 = Physics.Raycast(transform.position + ((sizeY / 2) * transform.right), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - ((sizeY / 2) * transform.right), transform.right, out hit2, range);

        if (hited1 || hited2)
        {
            if (hited1 && hit1.transform.tag == "Player" || hited2 && hit2.transform.tag == "Player")
            {
                watched = true;
                if (hited1 && hit1.transform.tag == "Player")
                {
                    transformPlayer = hit1.transform;
                }
                else
                {
                    transformPlayer = hit2.transform;
                }
            }
            else if ((hited1 && hit1.point.x - transform.position.x < (2 * sizeX) * direction ||
                hited2 && hit2.point.x - transform.position.x < (2 * sizeX) * direction ) && !stay)
            {
                stay = true;
                StartCoroutine(turn(0.2f));
            }
        }
        if (!stay)
        {
            Vector3 position = transform.position;
            position.x += speed * direction * Time.deltaTime;

            transform.position = position;
        }
        
    }
    private void Chase()
    {
        if(attack != null) { return; }
        stay = false;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,-transform.right, out hit, range))
        {
            if (hit.transform.tag == "Player" && !stay)
            {
                stay = true;
                StartCoroutine(turn(0.5f));//delay
            }
        }else if(Physics.Raycast(transform.position, transform.right, out hit, sizeX*3 )) 
        {
            print("ATACANDO");
            attack = StartCoroutine(DoAttack());
            print(attack != null);
            return;
        }
        else if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + 0.2f))
        {
            stay = true;
        }
        RaycastHit hit1;
        bool hited1 = Physics.Raycast(transform.position + ((sizeY / 2) * transform.right), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - ((sizeY / 2) * transform.right), transform.right, out hit2, range);

        if (hited1 || hited2)
        {
            if (hited1 && hit1.transform.tag == "Player" || hited2 && hit2.transform.tag == "Player")
            {
                watched = true;
                if (hited1 && hit1.transform.tag == "Player")
                {
                    enemyWatchedTimer = 0;
                }
                else
                {
                    enemyWatchedTimer = 0;
                }
            }
            else if (hited1 && hit1.point.x - transform.position.x < (2 * sizeX) * direction ||
                hited2 && hit2.point.x - transform.position.x < (2 * sizeX) * direction)
            {
                stay = true;
            }
        }
        if (!stay)
        {
            Vector3 position = transform.position;
            position.x += speed*2  * direction * Time.deltaTime;

            transform.position = position;
        }

    }

    IEnumerator DoAttack()
    {
        do
        {
            yield return new WaitForFixedUpdate();
        }
        while (attackTimer < 1);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, sizeX * 2)){
            if(hit.collider.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<PlayerController>().receiveDamage(damage);
            }
        }
        
        attack = null;
        attackTimer = 0;

    }

    IEnumerator turn(float time)
    {
        yield return new WaitForSeconds(time);
        Quaternion rotation = transform.rotation;

        if (direction == 1)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }
        direction *= -1;

        transform.rotation = rotation;
        stay = false;
    }
    public bool receiveDmg(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            return true;
        }
        return false;
    }

}
