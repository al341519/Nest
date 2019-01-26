using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArray : MonoBehaviour
{

    public List<Collider> enemies;

    // Update is called once per frame


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy")
        {
            enemies.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemies.Remove(other);
        }
    }
}
