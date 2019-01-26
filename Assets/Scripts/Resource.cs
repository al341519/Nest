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

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            print("Te doy " + resource + " c " + ammount);
            collider.gameObject.GetComponent<PlayerController>().setResource(resource, ammount);
            Destroy(this.gameObject);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

