using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepScript : MonoBehaviour
{
    public GameObject FootStep;

    // Start is called before the first frame update
    void Start()
    {
        FootStep.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 accumulatedVelocity = PlayerMovement.rb.velocity;
        if (accumulatedVelocity != Vector3.zero)
        {
            FootSteps();
        }
        else
        {
            StopFootSteps();
        }
    }

    private void StopFootSteps()
    {
        FootStep.SetActive(false);
    }

    private void FootSteps()
    {
        FootStep.SetActive(true);
    }
}
