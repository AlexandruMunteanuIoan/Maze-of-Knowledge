using UnityEngine;

public class GameManagerInitializer : MonoBehaviour
{
    public GameManagerConfig config;

    private void Start()
    {
        InitializeGameManager();
    }

    private void InitializeGameManager()
    {
        if (GameManager.Instance == null)
        {
            GameObject gameManagerObject = new GameObject("GameManager");
            GameManager gameManagerComponent = gameManagerObject.AddComponent<GameManager>();
            gameManagerComponent.config = config;
        }
    }
}
