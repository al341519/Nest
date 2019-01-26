using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    float health = 25;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
