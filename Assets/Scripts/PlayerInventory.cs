using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfQuestions {  get; private set; }
    
    public void QuestionsCollected()
    {
        NumberOfQuestions++;
    }
}
