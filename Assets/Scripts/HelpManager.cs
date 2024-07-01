using UnityEngine;

public class HelpManager : MonoBehaviour
{
    private Canvas helpCanvas; // Reference to the Canvas containing your help UI
    private PauseManager pauseManager;
    private bool isHelpActive = false; // Flag to track if help is currently active

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
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHelp(); // Toggle help state
        }
    }

    public void ShowHelp()
    {
        if (helpCanvas != null)
        {
            if (!pauseManager.IsPaused)
            {
                helpCanvas.enabled = true; // Show the help UI
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
            isHelpActive = false;
            HideHelp();
        }
        else
        {
            isHelpActive = true;
            ShowHelp();
        }
    }
}
