using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCollectedQuestions {  get; set; }
    public int NumberOfFinishedQuestions { get; set; }

    public List<Question> questions = new List<Question>();
    
    public void CollectQuestion(Question question)
    {
        questions ??= new List<Question>();

        questions.Add(question);
        NumberOfCollectedQuestions++;
    }

    public void AddFinished(int nrfinished)
    {
        this.NumberOfFinishedQuestions += nrfinished;
    }
}
