namespace Quaze.Models;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeLimit { get; set; }
    public Type QuestionType { get; set; }
    public List<string>? Choices { get; set; }
    public List<bool>? AnswerIndexes { get; set; }
    public string? Answer { get; set; }

    public enum Type {
        Choice, Open
    }
}