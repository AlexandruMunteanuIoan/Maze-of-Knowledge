using System.Collections.Generic;

using UnityEngine;

public class TestPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            //playerInventory.questions;
            if (playerInventory.questions.Count > 0)
            {
                QuizManager.Instance.StartQuiz(playerInventory.questions);
                playerInventory.questions = new List<Question>();
                playerInventory.NumberOfCollectedQuestions = 0;
            }
        }
    }
}
