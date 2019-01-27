/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum Class { Groot, Kappa, Wisp };

    float health = 25;
    public int direction = 1;
    float speed = 2;
    float sizeX, sizeY;
    float range = 10;
    float minDistance = 1;
    float enemyWatchedTimer = 0;
    bool watched = false;
    public int damage;
    Transform transformPlayer;
    Coroutine attack;

    float flyerOffSet = 0;

    float distanceShoot = 15;
    float timeBetweenShoots = 1;
    float shooterTimer;
    bool shooted;
    public GameObject fireBall;

    bool stay = false;
    public Class type;

    Animator anim;


    float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        if (type == Class.Wisp)
        {
            flyerOffSet = 1.5f;
            attackTimer = timeBetweenShoots;
        }
        sizeX = this.gameObject.GetComponent<Collider>().bounds.size.x / 2;
        sizeY = this.gameObject.GetComponent<Collider>().bounds.size.y / 2;
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!watched)
        {
            patrol();
        }

        if (watched)
        {
            if (type == Class.Groot)
            {
                Chase();
            }
            else if (type == Class.Kappa)
            {
                anim.SetBool("IsWalking", false);
                Block();
            }
            else if (type == Class.Wisp)
            {
                Debug.Log("Test");
                Aim();
            }

            enemyWatchedTimer += Time.deltaTime;
            if (enemyWatchedTimer >= 5)
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
        if(type == Class.Wisp)
        {
            attackTimer += Time.deltaTime;
        }
    }
    private void patrol()
    {
        //Debug.DrawRay(transform.position + (sizeX * transform.right), -transform.up);

        if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + 0.2f + flyerOffSet) && !stay)
        {
            stay = true;
            if (type != Class.Wisp)
                anim.SetBool("IsWalking", false);
            StartCoroutine(turn(0.8f));
        }


        RaycastHit hit1;
        bool hited1 = Physics.Raycast(transform.position + (((sizeY / 2)) * transform.right), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - (((sizeY / 2)) * transform.right), transform.right, out hit2, range);
        Debug.DrawRay(transform.position - (((sizeY / 2)) * transform.right), -transform.right*range);
        Debug.DrawRay(transform.position + (((sizeY / 2)) * transform.right), -transform.right * range);


        if (type == Class.Wisp)
        {
            hited1 = Physics.Raycast(transform.position, transform.right, out hit1, range);
            hited2 = Physics.Raycast(transform.position - ((flyerOffSet/2 + sizeY / 2) * transform.up), transform.right, out hit2, range);
            Debug.DrawRay(transform.position, transform.right * 100);
            Debug.DrawRay(transform.position - ((flyerOffSet / 2 + sizeY / 2) * transform.up), transform.right * 100);
        }

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
           else if ((hited1 && hit1.point.x - transform.position.x < (2 * sizeX) * direction )||
                (hited2 && hit2.point.x - transform.position.x < (2 * sizeX) * direction) && !stay)
            {
                stay = true;
                if (type != Class.Wisp)
                    anim.SetBool("IsWalking", false);
                StartCoroutine(turn(0.2f));
            }
        }
        if (!stay)
        {
            if (type != Class.Wisp)
            {
                anim.SetBool("IsWalking", true);
            }
            Vector3 position = transform.position;
            position.x += speed * direction * Time.deltaTime;

            transform.position = position;
        }

    }


    private void Block()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.right, out hit, range / 2))
        {
            if (hit.transform.tag == "Player" && !stay)
            {
                stay = true;
                StartCoroutine(turn(0.5f));//delay
            }
        }
        RaycastHit hit1;
        bool hited1 = Physics.Raycast(transform.position + ((sizeY / 2) * transform.right), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - ((sizeY / 2) * transform.right), transform.right, out hit2, range);

        if (hited1 || hited2)
        {
            if (hited1 && hit1.transform.tag == "Player" || hited2 && hit2.transform.tag == "Player")
            {
                enemyWatchedTimer = 0;
            }
        }
    }


    private void Chase()
    {

        if (attack != null) { return; }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.right, out hit, range))
        {
            if (hit.transform.tag == "Player" && !stay)
            {
                stay = true;
                anim.SetBool("IsWalking", false);
                StartCoroutine(turn(0.5f));//delay
            }
        }
        else if (Physics.Raycast(transform.position, transform.right, out hit, sizeX * 3))
        {
            print("ATACANDO");
            enemyWatchedTimer = 0;
            attack = StartCoroutine(DoAttack());
            print(attack != null);
            return;
        }
        else if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + flyerOffSet + 0.2f))
        {
            stay = true;
        }
        RaycastHit hit1;
        bool hited1 = Physics.Raycast(transform.position + ((sizeY / 2) * transform.right), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - ((sizeY / 2) * transform.right), transform.right, out hit2, range);
        Debug.DrawRay(transform.position - (((sizeY / 2)) * transform.right), transform.right * range);
        Debug.DrawRay(transform.position + (((sizeY / 2)) * transform.right), transform.right * range);

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
                anim.SetBool("IsWalking", false);
            }
        }
        if (!stay)
        {
            anim.SetBool("IsWalking", true);
            Vector3 position = transform.position;
            position.x += speed * 2 * direction * Time.deltaTime;

            transform.position = position;
        }

    }

    private void Aim()
    {
        RaycastHit bHit1;
        RaycastHit bHit2;
        bool bHited1 = Physics.Raycast(transform.position, -transform.right, out bHit1, range);
        bool bHited2 = Physics.Raycast(transform.position - ((flyerOffSet / 2 + sizeY / 2) * transform.up), -transform.right, out bHit2, range);

        RaycastHit hit1;
        RaycastHit hit2;
        bool hited1 = Physics.Raycast(transform.position, transform.right, out hit1, range);
        bool hited2 = Physics.Raycast(transform.position - ((flyerOffSet / 2 + sizeY / 2) * transform.up), transform.right, out hit2, range);
        if (bHited1 || bHited2)
        {
            if ((bHited1 && bHit1.transform.tag == "Player" || bHited2 && bHit2.transform.tag == "Player") && !stay)
            {
                stay = true;
                StartCoroutine(turn(0.5f));//delay
            }
        }
        else if ((Physics.Raycast(transform.position, transform.right, out hit1, distanceShoot) && hit1.transform.tag == "Player") && (attackTimer >= timeBetweenShoots) && attack == null)
        {
            anim.SetTrigger("IsAttaking");
 
            print("ATACANDO");
            enemyWatchedTimer = 0;
            attack = StartCoroutine(DoShoot());
            return;
        }        
        else if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + flyerOffSet + 0.2f))
        {
            stay = true;
        }
        else if ((hited1 && hit1.point.x - transform.position.x < distanceShoot * direction) ||
                    (hited2 && hit2.point.x - transform.position.x < distanceShoot * direction))
        {
            stay = true;
        }        

        if (!stay)
        {
            Vector3 position = transform.position;
            position.x += speed * 2 * direction * Time.deltaTime;

            transform.position = position;
        }

    }


    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.GetMask("Water"))
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    IEnumerator DoAttack()
    {
        do
        {
            yield return new WaitForFixedUpdate();
        }
        while (attackTimer < 0.5f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, sizeX * 2))
        {
            if (hit.collider.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<PlayerController>().receiveDamage(damage);
            }
        }

        attack = null;
        attackTimer = 0;

    }
    IEnumerator DoShoot()
    {
        while (attackTimer < timeBetweenShoots)
        {
            yield return new WaitForFixedUpdate();
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, distanceShoot))
        {
            if (hit.collider.tag == "Player")
            {
                print("pIUM");
                Instantiate(fireBall,transform);
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
        if (type != Class.Wisp)
            anim.SetTrigger("Hit");
        if (health <= 0)
        {
            return true;
        }
        return false;
        
    }

}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum Class { Groot, Kappa, Wisp };

    float health = 25;
    public int direction = 1;
    float speed = 2;
    float sizeX, sizeY;
    float range = 10;
    float minDistance = 1;
    float enemyWatchedTimer = 0;
    bool watched = false;
    public int damage;
    Transform transformPlayer;
    Coroutine attack;

    float flyerOffSet = 0;

    float distanceShoot = 15;
    float timeBetweenShoots = 1;
    float shooterTimer;
    bool shooted;
    public GameObject fireBall;

    bool stay = false;
    public Class type;
    Animator anim;




    float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        if (type == Class.Wisp)
        {
            flyerOffSet = 1.5f;
            attackTimer = timeBetweenShoots;
        }
        sizeX = this.gameObject.GetComponent<Collider>().bounds.size.x / 2;
        sizeY = this.gameObject.GetComponent<Collider>().bounds.size.y / 2;
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!watched)
        {
            patrol();
        }

        if (watched)
        {
            if (type == Class.Groot)
            {
                Chase();
            }
            else if (type == Class.Kappa)
            {
                anim.SetBool("IsWalking", false);
                Block();
            }
            else if (type == Class.Wisp)
            {
                Debug.Log("Test");
                Aim();
            }

            enemyWatchedTimer += Time.deltaTime;
            if (enemyWatchedTimer >= 5)
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
        if (type == Class.Wisp)
        {
            attackTimer += Time.deltaTime;
        }
    }
    private void patrol()
    {

        if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + 0.2f + flyerOffSet) && !stay)
        {
            stay = true;
            if (type != Class.Wisp)
                anim.SetBool("IsWalking", false);
            StartCoroutine(turn(0.8f));
        }


        RaycastHit hit1;
        Debug.DrawRay(transform.position + (((sizeY / 2)) * transform.up), transform.right * range);
        Debug.DrawRay(transform.position - (((sizeY / 2)) * transform.up), transform.right * range);

        bool hited1 = Physics.Raycast(transform.position + (((sizeY / 2)) * transform.up), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - (((sizeY / 2)) * transform.up), transform.right, out hit2, range);

        if (type == Class.Wisp)
        {
            hited1 = Physics.Raycast(transform.position, transform.right, out hit1, range);
            hited2 = Physics.Raycast(transform.position - ((flyerOffSet / 2 + sizeY / 2) * transform.up), transform.right, out hit2, range);
            Debug.DrawRay(transform.position, transform.right * 100);
            Debug.DrawRay(transform.position - ((flyerOffSet / 2 + sizeY / 2) * transform.up), transform.right * 100);
        }

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
            else if (direction == 1)
            {
                if ((hited1 && hit1.point.x - transform.position.x < (2 * sizeX) * direction ||
                hited2 && hit2.point.x - transform.position.x < (2 * sizeX) * direction) && !stay)
                {
                    float n1 = hit1.point.x - transform.position.x;
                    float n2 = (2 * sizeX) * direction;
                    float n3 = hit2.point.x - transform.position.x;
                    print(n1 + " " + n2 + " " + n3);

                    stay = true;
                    if (type != Class.Wisp)
                        anim.SetBool("IsWalking", false);
                    StartCoroutine(turn(0.2f));
                }
            }
            else
            {
                if ((hited1 && hit1.point.x - transform.position.x > (2 * sizeX) * direction ||
                hited2 && hit2.point.x - transform.position.x > (2 * sizeX) * direction) && !stay)
                {
                    float n1 = hit1.point.x - transform.position.x;
                    float n2 = (2 * sizeX) * direction;
                    float n3 = hit2.point.x - transform.position.x;
                    print(n1 + " " + n2 + " " + n3);

                    stay = true;
                    if (type != Class.Wisp)
                        anim.SetBool("IsWalking", false);
                    StartCoroutine(turn(0.2f));
                }
            }
        }
        if (!stay)
        {
            Vector3 position = transform.position;
            position.x += speed * direction * Time.deltaTime;
            anim.SetBool("IsWalking", true);

            transform.position = position;
        }

    }


    private void Block()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.right, out hit, range / 2))
        {
            if (hit.transform.tag == "Player" && !stay)
            {
                stay = true;
                StartCoroutine(turn(0.5f));//delay
            }
        }
        RaycastHit hit1;
        bool hited1 = Physics.Raycast(transform.position + ((sizeY / 2) * transform.right), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - ((sizeY / 2) * transform.right), transform.right, out hit2, range);

        if (hited1 || hited2)
        {
            if (hited1 && hit1.transform.tag == "Player" || hited2 && hit2.transform.tag == "Player")
            {
                enemyWatchedTimer = 0;
            }
        }
    }


    private void Chase()
    {
        if (attack != null) { return; }
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.right * sizeX * 3);
        if (Physics.Raycast(transform.position, -transform.right, out hit, range))
        {
            if (hit.transform.tag == "Player" && !stay)
            {
                stay = true;
                anim.SetBool("IsWalking", false);
                StartCoroutine(turn(0.5f));//delay
            }
        }
        if (Physics.Raycast(transform.position, transform.right, out hit, sizeX*2 ))
        {
            if (hit.transform.tag == "Player")
            {
                print("ATACANDO");
                anim.SetTrigger("IsAttacking");
                enemyWatchedTimer = 0;
                attack = StartCoroutine(DoAttack());
                print(attack != null);
                return;
            }
        }
        if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + flyerOffSet + 0.2f))
        {
            stay = true;
        }
        RaycastHit hit1;
        bool hited1 = Physics.Raycast(transform.position + ((sizeY / 2) * transform.up), transform.right, out hit1, range);
        RaycastHit hit2;
        bool hited2 = Physics.Raycast(transform.position - ((sizeY / 2) * transform.up), transform.right, out hit2, range);

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
            else if (direction == 1)
            {
                if ((hited1 && hit1.point.x - transform.position.x < (2 * sizeX) * direction ||
                hited2 && hit2.point.x - transform.position.x < (2 * sizeX) * direction) && !stay)
                { 
                    stay = true;
                    anim.SetBool("IsWalking", false);
                }
            }
            else
            {
                if ((hited1 && hit1.point.x - transform.position.x > (2 * sizeX) * direction ||
                hited2 && hit2.point.x - transform.position.x > (2 * sizeX) * direction) && !stay)
                {
                    anim.SetBool("IsWalking", false);
                    stay = true;
                }
            }
        }
        if (!stay)
        {
            Vector3 position = transform.position;
            position.x += speed * 2 * direction * Time.deltaTime;
            anim.SetBool("IsWalking", true);
            transform.position = position;
        }

    }

    private void Aim()
    {
        RaycastHit bHit1;
        RaycastHit bHit2;
        bool bHited1 = Physics.Raycast(transform.position, -transform.right, out bHit1, range);
        bool bHited2 = Physics.Raycast(transform.position - ((flyerOffSet / 2 + sizeY / 2) * transform.up), -transform.right, out bHit2, range);

        RaycastHit hit1;
        RaycastHit hit2;
        bool hited1 = Physics.Raycast(transform.position, transform.right, out hit1, range);
        bool hited2 = Physics.Raycast(transform.position - ((flyerOffSet / 2 + sizeY / 2) * transform.up), transform.right, out hit2, range);
        if (bHited1 || bHited2)
        {
            if ((bHited1 && bHit1.transform.tag == "Player" || bHited2 && bHit2.transform.tag == "Player") && !stay)
            {
                stay = true;
                StartCoroutine(turn(0.5f));//delay
            }
        }
        else if ((Physics.Raycast(transform.position, transform.right, out hit1, distanceShoot) && hit1.transform.tag == "Player") && (attackTimer >= timeBetweenShoots) && attack == null)
        {
            print("ATACANDO");
            enemyWatchedTimer = 0;
            attack = StartCoroutine(DoShoot());
            return;
        }
        else if (!Physics.Raycast(transform.position + (sizeX * transform.right), -transform.up, sizeY + flyerOffSet + 0.2f))
        {
            stay = true;
        }
        else if ((hited1 && hit1.point.x - transform.position.x < distanceShoot * direction) ||
                    (hited2 && hit2.point.x - transform.position.x < distanceShoot * direction))
        {
            stay = true;
        }

        if (!stay)
        {
            Vector3 position = transform.position;
            position.x += speed * 2 * direction * Time.deltaTime;

            transform.position = position;
        }

    }


    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.GetMask("Water"))
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    IEnumerator DoAttack()
    {
        do
        {
            yield return new WaitForFixedUpdate();
        }
        while (attackTimer < 3);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, sizeX * 2))
        {
            if (hit.collider.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<PlayerController>().receiveDamage(damage);
            }
        }

        attack = null;
        attackTimer = 0;

    }
    IEnumerator DoShoot()
    {
        while (attackTimer < timeBetweenShoots)
        {
            yield return new WaitForFixedUpdate();
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, distanceShoot))
        {
            if (hit.collider.tag == "Player")
            {
                print("pIUM");
                Instantiate(fireBall, transform);
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
        anim.SetTrigger("Hit");
        if (health <= 0)
        {
            return true;
        }
        return false;
    }

}