using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    private Canvas pauseCanvas; // Reference to the Canvas containing your pause UI
    private Canvas minimapCanvas;
    private Canvas gameTimerCanvas;
    private GameTimer gameTimer;
    private HelpManager helpManager;

    private bool minimapWasActive = true;
    private bool gameTimerWasActive = true;

    public bool isPaused = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of PauseManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

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

        // Store the current active state of minimap and game timer canvases
        minimapWasActive = minimapCanvas.enabled;
        gameTimerWasActive = gameTimerCanvas.enabled;

        // Pause book statistic controller
        if (BookStatisticController.Instance != null)
        {
            if (BookStatisticController.Instance.IsActive)
            {
                BookStatisticController.Instance.DeactivateBookStatisticCanvas();
            }
        }

        // Pause hint controller
        if (HintController.Instance != null)
        {
            if (HintController.Instance.isHintActive)
            {
                HintController.Instance.DeactivateHintCanvas();
            }
        }

        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = true; // Show the pause UI

            if (minimapCanvas != null)
            {
                minimapCanvas.enabled = false;
            }

            if (gameTimerCanvas != null)
            {
                gameTimerCanvas.enabled = false;
            }

            // Handle specific actions during pause if needed

            if (QuizManager.Instance != null && QuizManager.Instance.quizStarted)
            {
                QuizManager.Instance.quizCanvas.SetActive(false);
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

        gameTimer?.StopTimer();
        isPaused = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f; // Resume normal time scale
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Hide the mouse pointer

        // Resume book statistic controller
        if (BookStatisticController.Instance != null)
        {
            if (BookStatisticController.Instance.IsActive)
            {
                BookStatisticController.Instance.ActivateBookStatisticCanvas();
            }
        }

        // Resume hint controller
        if (HintController.Instance != null)
        {
            if (HintController.Instance.isHintActive)
            {
                HintController.Instance.ActivateHintCanvas();
            }
        }

        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false; // Hide the pause UI

            if (minimapCanvas != null && minimapWasActive)
            {
                minimapCanvas.enabled = true;
            }

            if (gameTimerCanvas != null && gameTimerWasActive)
            {
                gameTimerCanvas.enabled = true;
            }

            // Handle specific actions after resume if needed

            if (QuizManager.Instance != null && QuizManager.Instance.quizStarted)
            {
                Time.timeScale = 0f; // Pause the game
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; // Show the mouse pointer

                QuizManager.Instance.quizCanvas.SetActive(true);
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

        gameTimer?.StartTimer();
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
