using System.Collections.Generic;

using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    public List<Question> allQuestions;
    private List<Question> currentQuestions;

    private void Awake()
    {
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

    private List<Question> ParseDocument(TextAsset quizFile)
    {
        List<Question> allQuestionsLocal = new List<Question>();

        string QuestionText = string.Empty;
        List<string> QuestionAnswers = new List<string>();
        int QuestionAnswerIndex = -1;
        string QuestionHint = string.Empty;

        string[] lines = quizFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

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
                currentQuestion = null;
            }
            else if (line.StartsWith("**Intrebarea:**"))
            {
                QuestionText = line.Replace("**Intrebarea:**", "").Trim();
            }
            else if (line.StartsWith("1.") || line.StartsWith("2.") || line.StartsWith("3.") || line.StartsWith("4."))
            {
                QuestionAnswers.Add(line.Substring(line.IndexOf('.') + 1).Trim());
            }
            else if (line.StartsWith("**Indexul:**"))
            {
                QuestionAnswerIndex = int.Parse(line.Replace("**Indexul:**", "").Trim());
                currentQuestion = new Question(QuestionText, QuestionAnswers, QuestionAnswerIndex, QuestionHint);
                allQuestionsLocal.Add(currentQuestion);
                currentQuestion = null;
            }
        }

        if (currentQuestion != null)
        {
            allQuestionsLocal.Add(currentQuestion);
        }

        return allQuestionsLocal;
    }
}
