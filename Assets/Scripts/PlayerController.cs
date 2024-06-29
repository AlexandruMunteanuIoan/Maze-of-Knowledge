using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject PlayerObject;

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

    internal void SpawnPlayerInMaze(System.Collections.Generic.List<CellCenter> mazeCellsList, int wallSize, int mazeHeight, int mazeWidth)
    {
        CellCenter playerCellCenter;
        do
        {
            playerCellCenter = MazeGenerator.GetRandomAvailableCellCenter(mazeCellsList);
        } while (playerCellCenter.Position.x > ((mazeWidth - 2) * wallSize) ||
                 playerCellCenter.Position.z > ((mazeHeight - 2) * wallSize));
        

        if (playerCellCenter != null && !playerCellCenter.IsOccupied)
        {
            playerCellCenter.IsOccupied = true;
            Vector3 playerPosition = new Vector3(playerCellCenter.Position.x - wallSize / 2f, 0.5f, playerCellCenter.Position.z - wallSize / 2f);
            playerPosition -= new Vector3(wallSize / 2f, 0, wallSize / 2f);
            PlayerObject.transform.position = playerCellCenter.Position;
        }
        else
        {
            Debug.LogError("Player couldn't be moved where needed");
        }
    }
}
