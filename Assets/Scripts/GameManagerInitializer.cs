using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Ensure GameManager is initialized
        if (GameManager.Instance == null)
        {
            GameObject gameManager = new GameObject("GameManager");
            gameManager.AddComponent<GameManager>();
        }
    }
}
