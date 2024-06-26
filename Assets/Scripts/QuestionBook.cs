using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class QuestionBook : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if(playerInventory != null )
        {
            playerInventory.QuestionsCollected();
            gameObject.SetActive(false);
        }
    }
}
