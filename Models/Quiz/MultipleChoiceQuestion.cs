namespace Quaze.Models.Quiz;

public class MultipleChoiceQuestion : IQuestion
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeLimit { get; set; }
    public string[] Choices { get; set; }
    public int AnswerIndex { get; set; }
}