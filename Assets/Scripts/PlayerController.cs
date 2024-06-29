using System;
using System.Runtime.CompilerServices;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private GameObject PlayerObject;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Implement player movement logic
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Book"))
        {
            QuestionBook book = other.gameObject.GetComponent<QuestionBook>();
            ShowHint(book.GetHint());
            // Add book question to the question manager
        }
        else if (other.gameObject.CompareTag("Computer"))
        {
            // Show quiz interface
        }
    }

    private void ShowHint(string hint)
    {
        // Display hint to the player
    }

    internal void SpawnPlayerInMaze(System.Collections.Generic.List<CellCenter> mazeCellsList, int wallSize)
    {

        CellCenter playerCellCenter = MazeGenerator.GetRandomAvailableCellCenter(mazeCellsList);
        if (playerCellCenter != null)
        {
            playerCellCenter.IsOccupied = true;
            Vector3 playerPosition = new Vector3(playerCellCenter.Position.x - wallSize / 2f, 0.5f, playerCellCenter.Position.z - wallSize / 2f);
            GameObject playerObjectTemp = Instantiate(PlayerObject, playerPosition, Quaternion.identity);
        }
    }
}
