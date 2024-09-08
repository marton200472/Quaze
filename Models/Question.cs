namespace Quaze.Models;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TimeLimit { get; set; }
    public Type QuestionType { get; set; }
    public List<Choice>? Choices { get; set; } = new();
    public string? Answer { get; set; } = string.Empty;

    public class Choice
    {
        public string Text { get; set; } = string.Empty;
        public bool Valid { get; set; }
    }

    public enum Type {
        Choice, Open
    }
}