using System;
using System.Collections.Generic;

using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    public List<Question> allQuestions;
    private List<Question> currentQuestions;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of QuestionManager exists
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

    public void LoadQuestions(TextAsset quizFile)
    {
        // Load questions from a file, database, or other resource
        allQuestions = ParseDocument(quizFile);
    }

    public List<Question> SelectQuestions(int count)
    {
        currentQuestions = new List<Question>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allQuestions.Count);
            currentQuestions.Add(allQuestions[randomIndex]);
            allQuestions.RemoveAt(randomIndex);
        }
        return currentQuestions;
    }

    public List<Question> GetCurrentQuestions()
    {
        return currentQuestions;
    }

    public void CollectQuestion(Question question)
    {
        // Logic to handle when a question is collected by the player
    }

    public void StartQuiz()
    {
        // Show quiz UI
    }

    public void ProcessQuizResults(List<Question> unansweredQuestions)
    {
        // Handle unanswered questions
    }

    private List<Question> ParseDocument(TextAsset quizFile)
    {
        List<Question> allQuestionsLocal = new List<Question>();

        string QuestionText = string.Empty;
        List<string> QuestionAnswers = new List<string>();
        int QuestionAnswerIndex = -1;
        string QuestionHint = string.Empty;

        string[] lines = quizFile.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        Question currentQuestion = null;

        foreach (string line in lines)
        {
            if (line.StartsWith("**Hint:**"))
            {
                if (currentQuestion != null)
                {
                    allQuestionsLocal.Add(currentQuestion);
                }
                QuestionHint = line.Replace("**Hint:**", "").Trim();
                QuestionText = string.Empty;
                QuestionAnswers = new List<string>();
                QuestionAnswerIndex = -1;
                currentQuestion = null; // Reset current question
            }
            else if (line.StartsWith("**Întrebarea:**"))
            {
                QuestionText = line.Replace("**Întrebarea:**", "").Trim();
            }
            else if (line.StartsWith("1.") || line.StartsWith("2.") || line.StartsWith("3.") || line.StartsWith("4."))
            {
                QuestionAnswers.Add(line.Substring(3).Trim());
            }
            else if (line.StartsWith("**Indexul:**"))
            {
                QuestionAnswerIndex = int.Parse(line.Replace("**Indexul:**", "").Trim()) - 1;

                // Create a new Question object with collected data
                currentQuestion = new Question(QuestionText, QuestionAnswers, QuestionAnswerIndex, QuestionHint);
                allQuestionsLocal.Add(currentQuestion);
            }
        }

        // Add the last question if not already added
        if (currentQuestion != null && !allQuestionsLocal.Contains(currentQuestion))
        {
            allQuestionsLocal.Add(currentQuestion);
        }

        return allQuestionsLocal;
    }


    //private void GenerateQuestion()
    //{
    //    if (QnA.Count > 0)
    //    {
    //        currentQuestion = Random.Range(0, QnA.Count);
    //        QuestionText.text = QnA[currentQuestion].Question;
    //        SetAnswer();
    //    }
    //    else
    //    {
    //        GameOver();
    //    }
    //}

    //private void SetAnswer()
    //{
    //    for (int i = 0; i < options.Length; i++)
    //    {
    //        options[i].GetComponent<AnsersScript>().isCorrect = false;
    //        options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];

    //        if (QnA[currentQuestion].CorrectAnswer == i)
    //        {
    //            options[i].GetComponent<AnsersScript>().isCorrect = true;
    //        }
    //    }
    //}

    //public void Correct()
    //{
    //    score += 1;
    //    QnA.RemoveAt(currentQuestion);
    //    GenerateQuestion();
    //}

    //public void Wrong()
    //{
    //    QnA.RemoveAt(currentQuestion);
    //    GenerateQuestion();
    //}

    public void GameOver()
    {
        //QuizPanel.SetActive(false);
        //GoPanel.SetActive(true);
        //scoreTxt.text = $"{score}/{totalQ}";
    }

    //SpawnPlayer();

    //if (quizFile != null)
    //{
    //    ParseDocument(quizFile);
    //    SelectRandomQuestions();
    //    totalQ = QnA.Count;
    //    SpawnQuestionsInMaze();
    //}
    //else
    //{
    //    Debug.LogError("Quiz file is not assigned in the Inspector.");
    //}
}
