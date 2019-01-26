using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCanvas : MonoBehaviour
{
    public GameObject canvas;
    public Text branches, leaves;

    PlayerController player;
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
            if (Input.GetKeyDown(KeyCode.P))
            {
                canvas.SetActive(true);
                player.menu = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                canvas.SetActive(false);
                player.menu = false;
            }
            if (player.menu)
            {
                int ramitas, hojas;
                ramitas = Mathf.Clamp(10 - player.getResource("branch"), 0, 10);
                hojas = Mathf.Clamp(5 - player.getResource("leaf"), 0, 5);

                print(ramitas + "hojas " + hojas);
                print("tengo" + player.getResource("branch") + "hojas " + player.getResource("leaf"));

                if (ramitas == 0 && hojas == 0)
                {
                    interacteableButton(0, true);
                    branches.text = "Puede ser mejorado";
                }
                else
                {
                    branches.text = "Faltan " + ramitas + " de ramitas y " + hojas + " hojas." ;
                    interacteableButton(0, false);
                }

                ramitas = Mathf.Clamp(5 - player.getResource("branch"), 0, 5);
                hojas = Mathf.Clamp(10 - player.getResource("leaf"), 0, 10);
                if (ramitas == 0 && hojas == 0) {
                    interacteableButton(1, true);
                    leaves.text = "Puede ser mejorado";
                }
                else
                {
                    leaves.text = "Faltan " + ramitas + " de ramitas y " + hojas+ " hojas.";
                    interacteableButton(1, false);
                }

            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player") {
            open = true;
            player = col.gameObject.GetComponent<PlayerController>();
            player.currentHealth = player.maxHealth;
            
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            open = false;
        }
    }
    private void interacteableButton(int value, bool type)
    {
        canvas.transform.GetChild(value).GetComponent<Button>().interactable = type;
    }
}
