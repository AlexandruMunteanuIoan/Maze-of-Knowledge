using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloadScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnloadCurrentScene();
        }
    }

    public void UnloadCurrentScene()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Get all game objects in the scene
        GameObject[] allObjects = currentScene.GetRootGameObjects();

        // Destroy all game objects in the scene
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        // Load the menu scene
        SceneManager.LoadScene("MenuScene");
    }
}
