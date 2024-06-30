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
            elapsedTime += 1f;
            UpdateTimerText();
            Debug.Log("Timer Updated: " + elapsedTime);
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
