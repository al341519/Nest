using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash : MonoBehaviour
{

    const int DASHDISTANCE = 3;
    const float DASHSPEED = 40;
    const string WALLLAYER = "Water";
    const float DASHCOOLDOWN = 1;
    const float COMBOTIME = 1;
    const float ATTACKDURATION1 = 1;                //Tiempo que tarda la animación de ataque 1
    const float ATTACKDURATION2 = 1;                //Tiempo que tarda la animación de ataque 2
    const float ATTACKDURATION3 = 1;                //Tiempo que tarda la animación de ataque 3
    const float DMGTICKTIME = 0.2f;                 //Tiempo que tarda la animación en "aplicar el daño"
    readonly float[] ATTACKDMG = { 5, 5, 10 };

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

    private Rigidbody rigibody;

    


    // Start is called before the first frame update
    void Start()
    {
        sizeX = this.gameObject.GetComponent<Collider>().bounds.size.x;
        rigibody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCdHabilities();
        Debug.DrawRay(transform.position, transform.right*sizeX/2);



        if (Input.GetKeyDown(KeyCode.LeftShift) && dashed == false && attacking == false) {
            finalDashPosition = useDash(direction);                                                                 //Obtiene la posición final tras el dash
            if (finalDashPosition != transform.position )
            {
                StartCoroutine(MoveFromTo(transform, transform.position, finalDashPosition, DASHSPEED));            //Mueve personaje mediante el dash
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Z)&& attacking == false && dashing == false) {
            if (comboing) {
                attacking = true;
                attackNumber++;
                attackTimer = 0;
                print("attack: " + attackNumber);

                StartCoroutine(DoAttacks(attackNumber));
            }
            else
            {
                attacking = true;
                attackNumber = 1;
                print("attack: " + attackNumber);
                StartCoroutine(DoAttacks(attackNumber));
            }
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            direction = Input.GetAxis("Horizontal");
            velocity.x = Input.GetAxis("Horizontal") * 0.1f;

            rigibody.position = rigibody.position + velocity;
        }
        else
        {
            velocity.x = 0;
        }




    }


    public Vector3 useDash(float value) {
        Vector3 position;
        RaycastHit hit;
        //PONER FOR CON VARIOS RAYCAST
        if (value >= 0)
        {
            if (Physics.Raycast(transform.position, transform.right, out hit, DASHDISTANCE + sizeX/2, LayerMask.GetMask(WALLLAYER)))
            {
                position = hit.transform.position;
                position.x = position.x - sizeX;
                position.y = transform.position.y;
                return (position);
            }
            position = transform.position;
            position += transform.right * DASHDISTANCE;
            return position;
        }
        else {
            if (Physics.Raycast(transform.position, -transform.right, out hit, DASHDISTANCE + sizeX/2, LayerMask.GetMask(WALLLAYER)))
            {
                position = hit.transform.position;
                position.x = position.x + sizeX;
                position.y = transform.position.y;
                return (position);
            }
            position = transform.position;
            position -= transform.right * DASHDISTANCE;
            return position;
        }

    }

    private void CheckCdHabilities() {
        if (dashed == true)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= DASHCOOLDOWN)
            {
                dashTimer = 0;
                dashed = false;
            }
        }

        if (attacking || comboing) {
            attackTimer += Time.deltaTime;
            //print("Timer = " + attackTimer);

            if (attackNumber == 1 && attackTimer >= ATTACKDURATION1) {
                attacking = false;
                comboing = true;
                if (attackTimer >= ATTACKDURATION1 + COMBOTIME) {
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

    private void Attack(List<Collider> enemies,int attack) {
        List<Collider> deadEnemies = new List<Collider>();
        foreach (Collider enemy in enemies) {
            if (enemy.gameObject.GetComponent<Enemy>().receiveDmg(ATTACKDMG[attack-1]))           //Inflinge al enemigo la cantidad de daño apropiada al ataque attack
            {
                deadEnemies.Add(enemy);
            };               
        }
        foreach (Collider enemy in deadEnemies) {
            GetComponentInChildren<EnemyArray>().enemies.Remove(enemy);
                Destroy(enemy.gameObject);
        }
       

    }

    IEnumerator DoAttacks(int idAttack) {
        do
        {
            yield return new WaitForFixedUpdate();
        }
        while (attackTimer < DMGTICKTIME);

        enemies = GetComponentInChildren<EnemyArray>().enemies;            
        Attack(enemies,attackNumber);
        
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
}
