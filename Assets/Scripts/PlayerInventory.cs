using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfQuestions {  get; private set; }

    public List<Question> questions = new List<Question>();
    
    public void CollectQuestion(Question question)
    {
        questions ??= new List<Question>();

        questions.Add(question);
        NumberOfQuestions++;
    }
}
