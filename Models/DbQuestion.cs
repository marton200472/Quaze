namespace Quaze.Models;

public class DbQuestion
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeLimit { get; set; }
    public Type QuestionType { get; set; }
    public List<string>? Choices { get; set; }
    public List<bool>? AnswerIndexes { get; set; }
    public string? Answer { get; set; }

    public DbQuestion()
    {
        
    }

    public DbQuestion(IQuestion qu) {
        if (qu is OpenWordQuestion)
        {
            var q = qu as OpenWordQuestion;
            Title = q.Title;
            Description = q.Description;
            TimeLimit = q.TimeLimit;
            QuestionType = Type.Open;
            Answer = q.Answer;
        }
        else {
            var q = qu as MultipleChoiceQuestion;
            Title = q.Title;
            Description = q.Description;
            TimeLimit = q.TimeLimit;
            QuestionType = Type.Choice;
            Choices = q.Choices;
            AnswerIndexes = q.AnswerIndexs;
        }
        
    }

    public enum Type {
        Choice, Open
    }
}