using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private Canvas pauseCanvas; // Reference to the Canvas containing your pause UI
    private Canvas minimapCanvas;
    private Canvas gameTimerCanvas;
    private GameTimer gameTimer;
    private HelpManager helpManager;

    public static bool isPaused = false;

    void Start()
    {
        pauseCanvas = GameObject.Find("PauseCanvas").GetComponent<Canvas>();
        minimapCanvas = GameObject.Find("MinimapView").GetComponent<Canvas>();
        gameTimerCanvas = GameObject.Find("GameTimer").GetComponent<Canvas>();
        gameTimer = FindObjectOfType<GameTimer>();
        helpManager = FindObjectOfType<HelpManager>();

        // Hide the canvas at the start
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
            minimapCanvas.enabled = true;
            gameTimerCanvas.enabled = true;
        }
        else
        {
            Debug.LogError("Pause Canvas is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Check if the player presses the pause key
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause(); // Toggle pause state
        }
    }

    public void SetPause()
    {
        Time.timeScale = 0f; // Pause the game

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Show the mouse pointer

        if (pauseCanvas != null && minimapCanvas != null)
        {
            pauseCanvas.enabled = true; // Show the pause UI
            minimapCanvas.enabled = false;
            gameTimerCanvas.enabled = false;

            if (QuizManager.Instance != null)
            {
                if (QuizManager.quizStarted)
                {
                    QuizManager.Instance.quizCanvas.SetActive(false);
                }
            }

            if (helpManager != null && helpManager.IsHelpActive())
            {
                helpManager.HideHelp(); // Hide help if active
            }
        }
        else
        {
            Debug.LogError("Pause Canvas is not assigned in the Inspector!");
        }
        gameTimer.StopTimer();
        isPaused = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f; // Resume normal time scale
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Hide the mouse pointer

        if (pauseCanvas != null && minimapCanvas != null)
        {
            pauseCanvas.enabled = false; // Hide the pause UI

            if(minimapCanvas != null)
            {
                minimapCanvas.enabled = true; 
            }

            gameTimerCanvas.enabled = true;

            if (QuizManager.Instance != null)
            {
                if (QuizManager.quizStarted)
                {
                    Time.timeScale = 0f; // Pause the game
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true; // Show the mouse pointer

                    QuizManager.Instance.quizCanvas.SetActive(true);
                }
            }

            if (helpManager != null && helpManager.IsHelpActive())
            {
                helpManager.ShowHelp(); // Restore help if it was active before pause
            }
        }
        else
        {
            Debug.LogError("Pause Canvas is not assigned in the Inspector!");
        }

        gameTimer.StartTimer();
    }

    public void ExitMenu()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume normal time scale

        // Get all game objects in the scene, including those marked as DontDestroyOnLoad
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Destroy all game objects
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        // Load the menu scene
        SceneManager.LoadScene("InitializationScene");
    }

    // Function to toggle pause state and show/hide the pause UI
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            SetPause();
        }
    }

    public bool IsPaused
    {
        get { return isPaused; }
    }
}
