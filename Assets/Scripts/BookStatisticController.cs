using TMPro;

using UnityEngine;

public class BookStatisticController : MonoBehaviour
{
    public static BookStatisticController Instance { get; private set; }

    private static TextMeshProUGUI bookStatisticText;
    private static GameObject bookStatisticCanvas;

    private PlayerInventory playerInventory;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of BookStatisticController exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeBookStatisticElements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeBookStatisticElements()
    {
        // Find the book statistic canvas by name and cache it
        if (bookStatisticCanvas == null)
        {
            bookStatisticCanvas = GameObject.Find("BookStatisticCanvas");
            if (bookStatisticCanvas != null)
            {
                // Find the NrBooks child object and get its TextMeshProUGUI component
                GameObject nrBooksTransform = GameObject.Find("NrBooks");
                if (nrBooksTransform != null)
                {
                    bookStatisticText = nrBooksTransform.GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    Debug.LogWarning("NrBooks object not found in the BookStatisticCanvas.");
                }

                DeactivateBookStatisticCanvas();
            }
            else
            {
                Debug.LogWarning("BookStatisticCanvas not found in the scene.");
            }


            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        }
    }

    public void UpdateBookStatistic(string statistic)
    {
        if (bookStatisticText != null)
        {
            bookStatisticText.text = statistic;
        }
        else
        {
            Debug.LogWarning("BookStatisticText component is not assigned or found.");
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            if (playerInventory != null && GameManager.Instance != null)
            {
                UpdateBookStatistic((playerInventory.NumberOfCollectedQuestions + playerInventory.NumberOfFinishedQuestions).ToString() + " / " + GameManager.Instance.currentNumOfQuestions.ToString());
            }
        }
    }

    public void ActivateBookStatisticCanvas()
    {
        if (HelpManager.Instance.IsHelpActive())
        {
            HelpManager.Instance.HideHelp();
        }

        if(HintController.Instance.isHintActive)
        {
            HintController.Instance.DeactivateHintCanvas();
        }

        if (bookStatisticCanvas != null && !PauseManager.Instance.isPaused)
        {
            bookStatisticCanvas.SetActive(true);
            IsActive = true;
        }
    }

    public void DeactivateBookStatisticCanvas()
    {
        if (bookStatisticCanvas != null)
        {
            bookStatisticCanvas.SetActive(false);
            IsActive = false;
        }
    }
}
