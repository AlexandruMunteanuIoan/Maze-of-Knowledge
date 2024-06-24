using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnsersScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;

    public void Answer()
    {
        if (isCorrect)
        {
            quizManager.correct();
        }
        else
        {
            quizManager.wrong();
        }
    }

}