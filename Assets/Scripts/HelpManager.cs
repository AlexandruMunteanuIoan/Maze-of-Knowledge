using UnityEngine;

public class HelpManager : MonoBehaviour
{
    public static HelpManager Instance;
    private Canvas helpCanvas; // Reference to the Canvas containing your help UI
    private PauseManager pauseManager;

    private bool isHelpActive = false; // Flag to track if help is currently active

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of BookStatisticController exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        helpCanvas = GameObject.Find("HelpCanvas").GetComponent<Canvas>();
        pauseManager = FindObjectOfType<PauseManager>();

        // Hide the canvas at the start
        if (helpCanvas != null)
        {
            helpCanvas.enabled = false;
        }
        else
        {
            Debug.LogError("Help Canvas is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Check if the player presses the help key (e.g., F1)
        if (Input.GetKeyDown(KeyCode.H) &&
            !HintController.Instance.isHintActive)
        {
            ToggleHelp(); // Toggle help state
        }
    }

    public void ShowHelp()
    {
        if (helpCanvas != null)
        {
            if (HintController.Instance.isHintActive)
            {
                HintController.Instance.DeactivateHintCanvas();
            }
            if (BookStatisticController.Instance.IsActive)
            {
                BookStatisticController.Instance.DeactivateBookStatisticCanvas();
            }
            if (!pauseManager.IsPaused)
            {
                helpCanvas.enabled = true; // Show the help UI
                this.isHelpActive = true;
            }
        }
        else
        {
            Debug.LogError("Help Canvas is not assigned in the Inspector!");
        }
    }

    public void HideHelp()
    {
        if (helpCanvas != null)
        {
            if (!pauseManager.IsPaused)
            {
                helpCanvas.enabled = false; // Hide the help UI
                this.isHelpActive = false;
            }
        }
        else
        {
            Debug.LogError("Help Canvas is not assigned in the Inspector!");
        }
    }

    public bool IsHelpActive()
    {
        return isHelpActive;
    }

    // Function to toggle help state and show/hide the help UI
    public void ToggleHelp()
    {
        if (isHelpActive)
        {
            HideHelp();
        }
        else
        {
            ShowHelp();
        }
    }
}
