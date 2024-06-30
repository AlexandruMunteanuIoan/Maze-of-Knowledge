using System.Collections.Generic;

public class Question
{
    public string Text { get; private set; }
    public List<string> Answers { get; private set; }
    public int CorrectAnswerIndex { get; private set; }
    public string Hint { get; private set; }

    public Question(string text, List<string> answers, int correctAnswerIndex, string hint)
    {
        Text = text;
        Answers = answers;
        CorrectAnswerIndex = correctAnswerIndex;
        Hint = hint;
    }

    public bool CheckAnswer(int index)
    {
        return index == CorrectAnswerIndex;
    }

    public string GetHint()
    {
        return Hint;
    }
}
