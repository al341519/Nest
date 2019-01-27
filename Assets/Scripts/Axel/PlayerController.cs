﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    const int DASHDISTANCE = 3;
    const float DASHSPEED = 40;
    const string WALLLAYER = "Water";
    const float DASHCOOLDOWN = 0;
    const float COMBOTIME = 0.2f;
    const float ATTACKDURATION1 = 0.2f;                //Tiempo que tarda la animación de ataque 1
    const float ATTACKDURATION2 = 0.2f;                //Tiempo que tarda la animación de ataque 2
    const float ATTACKDURATION3 = 0.2f;                //Tiempo que tarda la animación de ataque 3
    const float DMGTICKTIME = 0.1f;                 //Tiempo que tarda la animación en "aplicar el daño"
    public float[] attackDmg = { 5, 5, 10 };

    const float MOVEMENTSPEED = 0.1f;

    Vector3 velocity = Vector3.zero;

    private float dashTimer;
    private bool dashing;
    private bool dashed;

    private Vector3 finalDashPosition;

    private float sizeX;

    private float direction;

    private float attackTimer;
    private bool attacking;
    private bool comboing;
    private int attackNumber;

    private List<Collider> enemies;

    public bool menu; 


    public float speed = 0.1f;
    public float jumpSpeed = 0.07f;
    public float gravity = 8f;
    public bool grounded;

    // protected Rigidbody rb;
    protected CharacterController CharCtrl;

    public static int resourceLeaf;
    public static int resourceBranch;

    public int maxHealth = 50;
    public int currentHealth;

    public Material dissolveMat;
    Material currentMat;
    float dValue;
    bool gameOver;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;// maxHealth;
        sizeX = this.gameObject.GetComponent<Collider>().bounds.size.x;
        grounded = true;
        CharCtrl = GetComponent<CharacterController>();
        menu = false;

        gameOver = false;
        dValue = 2;
        currentMat = GetComponent<MeshRenderer>().material;
        //receiveDamage(1);

        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(menu) { return; }
        CheckCdHabilities();
        Debug.DrawRay(transform.position, transform.right * (sizeX / 2));
        //Debug.Log("posicion " + transform.position.x + " right : " + transform.right.x + "size " + sizeX / 2);

        if (gameOver)
        {
            dValue -= Time.deltaTime;
           // currentMat.SetFloat("DissolveValue", dValue);
            if (dValue <= -0.2)
            {
                //pantalla gameover
                SceneManager.LoadScene(1);
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) && dashed == false && attacking == false)
            {
                anim.SetTrigger("Dash");
                finalDashPosition = useDash(direction);                                                                 //Obtiene la posición final tras el dash
                if (finalDashPosition != transform.position)
                {
                    StartCoroutine(MoveFromTo(transform, transform.position, finalDashPosition, DASHSPEED));            //Mueve personaje mediante el dash

                }
            }

            if (Input.GetKeyDown(KeyCode.K) && attacking == false && dashing == false)
            {

                if (comboing)
                {
                    attacking = true;
                    attackNumber++;
                    if(attackNumber == 2)
                    {
                        anim.SetTrigger("Combo 1");
                    }
                    else
                    {
                        anim.SetTrigger("Combo 2");
                    }
                    attackTimer = 0;
                    print("attack: " + attackNumber);

                    StartCoroutine(DoAttacks(attackNumber));
                }
                else
                {
                    anim.SetTrigger("IsAttacking");
                    attacking = true;
                    attackNumber = 1;
                    print("attack: " + attackNumber);
                    StartCoroutine(DoAttacks(attackNumber));
                }
            }
            if (dashing == false)
            {
                Vector3 move = Vector3.zero;
                velocity.x = 0f;
                if (!attacking)
                {
                    if (Input.GetAxis("Horizontal") != 0)
                    {
                        anim.SetBool("IsWalking", true);

                        direction = Input.GetAxis("Horizontal");
                        if (direction > 0)
                        {
                            Quaternion rotation = transform.rotation;
                            rotation.y = 0;
                            transform.rotation = rotation;
                        }
                        else if (direction < 0)
                        {
                            Quaternion rotation = transform.rotation;
                            rotation.y = 180;
                            transform.rotation = rotation;
                        }

                        velocity.x = Input.GetAxis("Horizontal") * speed;
                    }
                    else
                    {
                        anim.SetBool("IsWalking", false);
                    }
                }
                grounded = CharCtrl.isGrounded;


                if (Input.GetButtonDown("Jump") && grounded)
                {
                    anim.SetTrigger("Jump");
                    grounded = false;
                    velocity.y = jumpSpeed;
                }

                if (!grounded)
                {

                    velocity.y -= gravity * Time.deltaTime;

                }
                CharCtrl.Move(velocity);
            }
        }

    }


    public Vector3 useDash(float value)
    {
        Vector3 position;
        RaycastHit hit;
        //PONER FOR CON VARIOS RAYCAST

        if (Physics.Raycast(transform.position, transform.right, out hit, DASHDISTANCE + sizeX / 2, LayerMask.GetMask(WALLLAYER)))
        {
            position = hit.point;
            Debug.Log("test");
            Debug.Log(position.x);
            if (value >= 0)
            {
                position.x = position.x - sizeX / 2;
            }
            else {
                position.x = position.x + sizeX / 2;
            }
            position.y = transform.position.y;
            return (position);
        }
        else {
            Debug.Log("posicion: " + transform.position.x + " tamaño: " + DASHDISTANCE + " tamaño2 : " + sizeX / 2);
            position = transform.position;
            position += transform.right * DASHDISTANCE;
            return position;
        }

    }

    private void CheckCdHabilities()
    {
        if (dashed == true)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= DASHCOOLDOWN)
            {
                dashTimer = 0;
                dashed = false;
            }
        }

        if (attacking || comboing)
        {
            attackTimer += Time.deltaTime;
            anim.SetFloat("TempCombo", attackTimer);
            //print("Timer = " + attackTimer);

            if (attackNumber == 1 && attackTimer >= ATTACKDURATION1)
            {
                attacking = false;
                comboing = true;
                if (attackTimer >= ATTACKDURATION1 + COMBOTIME)
                {
                    comboing = false;
                    attackTimer = 0;
                }
            }

            else if (attackNumber == 2 && attackTimer >= ATTACKDURATION2)
            {
                attacking = false;
                comboing = true;
                if (attackTimer >= ATTACKDURATION2 + COMBOTIME)
                {
                    comboing = false;
                    attackTimer = 0;
                }
            }

            else if (attackNumber == 3 && attackTimer >= ATTACKDURATION3)
            {
                attacking = false;
                comboing = false;
                attackTimer = 0;
            }
        }
    }

    private void Attack(List<Collider> enemies, int attack)
    {
        List<Collider> deadEnemies = new List<Collider>();
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<Enemy>().receiveDmg(attackDmg[attack - 1]))           //Inflinge al enemigo la cantidad de daño apropiada al ataque attack
            {
                deadEnemies.Add(enemy);
            };
        }
        foreach (Collider enemy in deadEnemies)
        {
            GetComponentInChildren<EnemyArray>().enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }


    }

    IEnumerator DoAttacks(int idAttack)
    {
        do
        {
            yield return new WaitForFixedUpdate();
        }
        while (attackTimer < DMGTICKTIME);

        enemies = GetComponentInChildren<EnemyArray>().enemies;
        Attack(enemies, attackNumber);

    }

    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        dashing = true;
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
        dashing = false;
        dashed = true;
    }
    public void setResource(string res, int ammount)
    {
        if (res == "leaf")
        {
            resourceLeaf += ammount;
        }
        else if (res == "branch")
        {
            resourceBranch += ammount;
        }
    }

    public int getResource(string res)
    {
        if (res == "leaf")
        {
            return (resourceLeaf);
        }
        else if (res == "branch")
        {
            return (resourceBranch);
        }
        return 0;
    }

    public void increaseAttack(float ammount)
    {
        if (resourceBranch >= 10 && resourceLeaf >= 5)
        {
            resourceBranch -= 10;
            resourceLeaf -= 5;
            for (int i = 0; i < attackDmg.Length; i++){
                attackDmg[i] *= ammount;
            }
        }

    }
    public void increaseHealth(int ammount)
    {
        if (resourceBranch >= 5 && resourceLeaf >= 10)
        {
            resourceBranch -= 5;
            resourceLeaf -= 10;
            maxHealth += ammount;
        }
    }
    public void receiveDamage(int damage)
    {
        if (!dashing)
        {
            anim.SetTrigger("Hit");
            currentHealth -= damage;
            if (currentHealth <= 0)
            { 
    //            currentMat = dissolveMat;
                gameOver = true;
            }
            StopCoroutine("DoAttacks");


        }
    }


}

