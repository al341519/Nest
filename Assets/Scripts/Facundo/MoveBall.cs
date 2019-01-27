using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{


    float time;
    Vector3 position;
    float speed = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > 3)
        {
            Destroy(this.gameObject);
        }
        position = transform.position;
        position.x += speed * Time.deltaTime;
        transform.position = position;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerController>().receiveDamage(10);
        }
    }

}
