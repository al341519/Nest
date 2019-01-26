using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {

        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Egg") {
            open = true;
        }
    }

    private void OnTrigger
}
