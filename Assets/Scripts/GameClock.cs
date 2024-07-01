using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the Text component to display the time
    private float elapsedTime;
    private bool isRunning;
    private Coroutine timerCoroutine;

    void Start()
    {
        // Initialize variables
        elapsedTime = 0f;
        isRunning = false;
        UpdateTimerText();
        StartTimer();
    }

    public void StartTimer()
    {
        if (!isRunning)
        {
            Debug.Log("Timer Started");
            isRunning = true;
            timerCoroutine = StartCoroutine(UpdateTimer());
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            Debug.Log("Timer Stopped");
            isRunning = false;
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }
    }

    public void ResetTimer()
    {
        Debug.Log("Timer Reset");
        StopTimer();
        elapsedTime = 0f;
        UpdateTimerText();
    }

    private IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            yield return new WaitForSecondsRealtime(1f); // Wait for one second in real-time
            elapsedTime += 1f; // Increase the elapsed time by one second
            UpdateTimerText(); // Update the timer text on the UI
            Debug.Log("Timer Updated: " + elapsedTime); // Log the updated timer value
        }
    }


    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
