using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    public Text questionText;
    public List<Button> answerButtons;
    private Question currentQuestion;

    public void ShowQuestion(Question question)
    {
        currentQuestion = question;
        questionText.text = question.Text;
        // Populate answer buttons with options
    }

    public void SubmitAnswer(int answerIndex)
    {
        bool isCorrect = currentQuestion.CheckAnswer(answerIndex);
        // Handle answer submission
    }

    public void FinishQuiz()
    {
        // Handle end of quiz, return unanswered questions to QuestionManager
    }
}
