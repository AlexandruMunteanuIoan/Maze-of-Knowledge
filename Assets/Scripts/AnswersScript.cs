using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnsersScript : MonoBehaviour
{
    public bool isCorrect = false;
    public MazeGenerator quizManager;

    public void Answer()
    {
        if (isCorrect)
        {
            quizManager.Correct();
        }
        else
        {
            quizManager.Wrong();
        }
    }

}