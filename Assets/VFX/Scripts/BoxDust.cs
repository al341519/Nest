using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class BoxDust : MonoBehaviour
{
    VisualEffect[] visualEffects;
    Rigidbody rb;

    private void Start()
    {
        visualEffects = GetComponentsInChildren<VisualEffect>();
        rb = GetComponent<Rigidbody>();
        RunAnimation(false);
    }

    void Update()
    {
        if (rb.velocity.sqrMagnitude != 0)
        {
            RunAnimation(true);
        }
        else
        {
            RunAnimation(false);
        }
    }

    void RunAnimation(bool b)
    {
        foreach (VisualEffect ve in visualEffects)
        {
            ve.enabled = b;
        }
    }
}
