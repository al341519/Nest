using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    GameObject player;
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
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                canvas.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player") {
            open = true;
            player = col.gameObject;
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
