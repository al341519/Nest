using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{

    public int ammount;
    public string resource;
    // Start is called before the first frame update
    void Start()
    {

    }

    void onTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {

            collider.gameObject.GetComponent<PlayerController>().setResource(resource, ammount);


        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

