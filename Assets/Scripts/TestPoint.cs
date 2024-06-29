using UnityEngine;

public class TestPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            //playerInventory.questions;
        }
    }
}
