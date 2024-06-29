using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        // Open the settings menu scene or UI panel
        SceneManager.LoadScene("SettingsScene");
    }

    public void ShowCredits()
    {
        // Show the credits screen
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
}
