using UnityEngine;

public class QuestionBook : MonoBehaviour
{
    private Question question;

    public void SetQuestion(Question newQuestion)
    {
        question = newQuestion;
    }

    public string GetHint()
    {
        return question.GetHint();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        // Show hint to the player
    //        string hint = GetHint();
    //        UIManager.Instance.ShowHint(hint);

    //        // Add question to the QuestionManager or mark as collected
    //        QuestionManager.Instance.CollectQuestion(question);

    //        // Destroy the book after collection
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.CollectQuestion(this.question);
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }
}
