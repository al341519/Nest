using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCanvas : MonoBehaviour
{
    public GameObject canvas;
    bool player;
    bool open;
    // Start is called before the first frame update
    void Start()
    {
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                canvas.SetActive(true);
                player = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                canvas.SetActive(false);
                player = false;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player") {
            open = true;
            player = col.gameObject.GetComponent<PlayerController>().menu;
            
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            open = false;
        }
    }
}
