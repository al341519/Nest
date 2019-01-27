using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode { In,Out}

public class DissolveAnimation : MonoBehaviour
{

    public Material dissolveMaterial;
    Material targetMaterial;

    public float _DissolveTime = 2;
    float currentTime = 0, currentDissolveValue = 0;

    bool isAnimating = false;
    PlayMode playMode;

    void Start()
    {
        targetMaterial = GetComponent<MeshRenderer>().material;
        //Play(PlayMode.In);
    }
    private void Awake()
    {
        Play(PlayMode.In);
    }

    private void Update()
    {
        if (isAnimating)
        {
            
            print("CurrentTime: " + currentTime);
            print("CurrentDissolve: " + currentDissolveValue);
            currentTime += Time.deltaTime;
            switch (playMode)
            {
                case PlayMode.In:
                    currentDissolveValue = Mathf.Lerp(-0.2f, 2, Mathf.InverseLerp(0, _DissolveTime, currentTime));
                    break;
                case PlayMode.Out:
                    currentDissolveValue = Mathf.Lerp(2, -0.2f, Mathf.InverseLerp(0, _DissolveTime, currentTime));
                    break;
            }
            if (currentTime >= _DissolveTime)
            {
                print("Entering IF");
                isAnimating = false;
                GetComponent<MeshRenderer>().material.SetFloat("_DisslveValue", currentDissolveValue);
                GetComponent<MeshRenderer>().material = targetMaterial;
                currentTime = 0;
                currentDissolveValue = 0;
            }
        }
    }

    public void Play(PlayMode pM)
    {
        if (!isAnimating)
        {
            isAnimating = true;
            GetComponent<MeshRenderer>().material = dissolveMaterial;
            playMode = pM;
        }
    }
    
}
