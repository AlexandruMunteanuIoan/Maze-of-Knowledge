using System;
using UnityEngine;

public class FootStepScript : MonoBehaviour
{
    public GameObject FootStep;
    private Boolean Active = false;

    // Start is called before the first frame update
    void Start()
    {
        FootStep.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 accumulatedVelocity = PlayerMovement.rb.velocity;
        accumulatedVelocity.y = 0f; 
        if (accumulatedVelocity != Vector3.zero && 
            !PauseManager.Instance.isPaused && 
            !QuizManager.Instance.quizStarted &&
            !HintController.Instance.isHintActive)
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
        if (Active)
        {
            FootStep.SetActive(false);
            FootStep.GetComponent<AudioSource>().Pause();
            Active = !Active;
        }
    }

    private void FootSteps()
    {
        if (!Active)
        {
            FootStep.SetActive(true);
            FootStep.GetComponent<AudioSource>().Play();
            Active = !Active;
        }
    }
}
