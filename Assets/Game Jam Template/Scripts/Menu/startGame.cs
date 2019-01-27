using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    public GameObject canvas;

    public void OnClick()
    {
        SceneManager.LoadScene(1);
        canvas.SetActive(false);
    }
}
