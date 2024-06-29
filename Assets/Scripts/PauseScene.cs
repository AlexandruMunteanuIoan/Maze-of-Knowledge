using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    // Method to resume the game by reloading the GameScene
    public void Play()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    // Method to quit the game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("The Player has Quit the game");
    }
}
