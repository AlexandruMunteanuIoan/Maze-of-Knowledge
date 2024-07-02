using TMPro;

using UnityEngine;

public class HintController : MonoBehaviour
{
    public static HintController Instance { get; private set; }

    private static TextMeshProUGUI hintText;
    private static GameObject hintCanvas;

    public bool isHintActive = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of HintController exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeHintElements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Check for ESC key press to deactivate hint canvas and resume the game
        if (isHintActive && Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateHintCanvas();
        }
    }

    private void InitializeHintElements()
    {
        // Find the hint canvas by name and cache it
        if (hintCanvas == null)
        {
            hintCanvas = GameObject.Find("HintCanvas");
            if (hintCanvas != null)
            {
                hintText = hintCanvas.GetComponentInChildren<TextMeshProUGUI>();
                DeactivateHintCanvas();
            }
            else
            {
                Debug.LogWarning("HintCanvas not found in the scene.");
            }
        }
    }

    public void ShowHint(string hint)
    {
        if (hintText != null)
        {
            hintText.text = hint;
            ActivateHintCanvas();
        }
        else
        {
            Debug.LogWarning("HintText component is not assigned or found.");
        }
    }

    public void ActivateHintCanvas()
    {
        if (hintCanvas != null)
        {
            if (BookStatisticController.Instance.IsActive)
            {
                BookStatisticController.Instance.DeactivateBookStatisticCanvas();
            }
            if(HelpManager.Instance.IsHelpActive())
            {
                HelpManager.Instance.HideHelp();
            }
            hintCanvas.SetActive(true);
            PauseGame();
        }
    }

    public void DeactivateHintCanvas()
    {
        if (hintCanvas != null)
        {
            hintCanvas.SetActive(false);
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        isHintActive = true;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isHintActive = false;
        Time.timeScale = 1f;
    }
}
