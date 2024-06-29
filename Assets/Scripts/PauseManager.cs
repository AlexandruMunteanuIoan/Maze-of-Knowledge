using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public TextMeshProUGUI pauseText; // Referință la TextMeshPro UI element
    private bool isPaused = false;

    void Start()
    {
        // Ascunde textul la început
        pauseText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Verifică dacă jucătorul apasă tasta de pauză (exemplu: Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause("Game Paused!"); // Mesaj de pauză implicit
        }
    }

    // Funcție pentru a pune jocul pe pauză și a afișa mesajul
    public void TogglePause(string message)
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f; // Pune jocul pe pauză
            pauseText.text = message; // Setează mesajul în TextMeshPro
            pauseText.gameObject.SetActive(true); // Afișează textul
        }
        else
        {
            Time.timeScale = 1f; // Reia jocul
            pauseText.gameObject.SetActive(false); // Ascunde textul
        }
    }
}

