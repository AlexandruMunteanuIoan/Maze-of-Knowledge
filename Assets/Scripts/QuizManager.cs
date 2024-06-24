using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class QuizManager : MonoBehaviour
{
    public static List<QandA> QnA = new List<QandA>();
    public GameObject[] options;
    public int currentQuestion;
    public TextAsset quizFile;  // Variabilă publică pentru fișierul de întrebări

    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI scoreTxt;

    public GameObject QuizPanel;
    public GameObject GoPanel;

    int totalQ = 0;
    public int score;

    private void Start()
    {
        if (quizFile != null)
        {
            ParseDocument(quizFile);
            SelectRandomQuestions();
            totalQ = QnA.Count;
            GoPanel.SetActive(false);
            generateQuestion();
        }
        else
        {
            Debug.LogError("Quiz file is not assigned in the Inspector.");
        }
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionText.text = QnA[currentQuestion].Question;
            SetAnswer();
        }
        else
        {
            GameOver();
        }
    }

    void SetAnswer()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnsersScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i)
            {
                options[i].GetComponent<AnsersScript>().isCorrect = true;
            }
        }
    }

    public void correct()
    {
        score += 1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void GameOver()
    {
        QuizPanel.SetActive(false);
        GoPanel.SetActive(true);
        scoreTxt.text = score + "/" + totalQ;
    }

    public void exit()
    {
        Application.Quit();
        Debug.Log("Application has been closed.");
    }

    public void wrong()
    {
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    void ParseDocument(TextAsset quizFile)
    {
        QnA.Clear();

        string[] lines = quizFile.text.Split('\n');
        QandA currentQandA = null;

        foreach (string line in lines)
        {
            if (line.StartsWith("**Hint:**"))
            {
                if (currentQandA != null)
                {
                    QnA.Add(currentQandA);
                }
                currentQandA = new QandA();
                currentQandA.Hint = line.Replace("**Hint:**", "").Trim();
            }
            else if (line.StartsWith("**Întrebarea:**"))
            {
                currentQandA.Question = line.Replace("**Întrebarea:**", "").Trim();
            }
            else if (line.StartsWith("**Răspunsuri:**"))
            {
                currentQandA.Answers = new string[4];
            }
            else if (line.StartsWith("1."))
            {
                currentQandA.Answers[0] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("2."))
            {
                currentQandA.Answers[1] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("3."))
            {
                currentQandA.Answers[2] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("4."))
            {
                currentQandA.Answers[3] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("**Indexul:**"))
            {
                currentQandA.CorrectAnswer = int.Parse(line.Replace("**Indexul:**", "").Trim()) - 1;
            }
        }

        if (currentQandA != null)
        {
            QnA.Add(currentQandA);
        }
    }

    void SelectRandomQuestions()
    {
        List<QandA> selectedQuestions = new List<QandA>();
        int numberOfQuestions = Mathf.Min(10, QnA.Count);
        HashSet<int> selectedIndices = new HashSet<int>();

        while (selectedIndices.Count < numberOfQuestions)
        {
            int randomIndex = Random.Range(0, QnA.Count);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                selectedQuestions.Add(QnA[randomIndex]);
            }
        }

        QnA = selectedQuestions;
    }
}