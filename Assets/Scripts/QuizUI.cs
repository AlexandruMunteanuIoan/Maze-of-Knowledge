using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<TextMeshProUGUI> answerTexts;

    private Question currentQuestion;
    private int currentQuestionIndex;
    private List<Question> questions;

    public void StartQuiz(List<Question> questions)
    {
        this.questions = questions;
        currentQuestionIndex = 0;
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            currentQuestion = questions[currentQuestionIndex];
            questionText.text = currentQuestion.Text;

            for (int i = 0; i < answerTexts.Count; i++)
            {
                Button answerButton = answerTexts[i].GetComponent<Button>();
                if (i < currentQuestion.Answers.Count)
                {
                    //answerButton.gameObject.SetActive(true);
                    answerTexts[i].text = currentQuestion.Answers[i];
                    int index = i;
                    answerButton.onClick.RemoveAllListeners();
                    answerButton.onClick.AddListener(() => OnAnswerSelected(index));
                }
                else
                {
                    answerButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            QuizManager.Instance.FinishQuiz();
        }
    }

    private void OnAnswerSelected(int index)
    {
        QuizManager.Instance.SubmitAnswer(currentQuestion, index);
        currentQuestionIndex++;
        ShowQuestion();
    }
}
