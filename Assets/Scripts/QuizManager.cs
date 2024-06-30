using System.Collections.Generic;

using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }
    public static bool quizStarted = false;
    public GameObject quizCanvas;
    private QuizUI quizUI;
    private List<Question> currentQuestions;
    private List<Question> incorrectQuestions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if(quizCanvas != null)
            {
                quizCanvas.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartQuiz(List<Question> questions)
    {
        Time.timeScale = 0; // Stop the game

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Show the mouse pointer

        currentQuestions = questions;
        incorrectQuestions = new List<Question>();

        // Ensure we get the QuizUI component from the Canvas
        quizCanvas.SetActive(true);
        quizStarted = true;

        quizUI = quizCanvas.GetComponentInChildren<QuizUI>();
        if (quizUI != null)
        {
            quizUI.StartQuiz(questions); // Start the quiz in the UI
        }
        else
        {
            Debug.LogError("QuizUI component not found in the quizCanvas GameObject or its children.");
        }
    }

    public void SubmitAnswer(Question question, int answerIndex)
    {
        if (!question.CheckAnswer(answerIndex))
        {
            incorrectQuestions.Add(question);
        }
    }

    public void FinishQuiz()
    {
        quizStarted = false;
        quizCanvas.SetActive(false);

        Time.timeScale = 1; // Continue the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Show the mouse pointer

        GameManager.Instance.OnQuizCompleted(incorrectQuestions);
    }
}
