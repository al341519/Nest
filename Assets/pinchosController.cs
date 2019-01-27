using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinchosController : MonoBehaviour
{
    public int damage;
    private float timeBetweenDmgTick;
    private float timer;
    private bool dmg;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dmg)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenDmgTick)
            {
                player.GetComponent<PlayerController>().receiveDamage(damage);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            dmg = true;
            player = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            dmg = false;
            player = null;
        }
    }
}
