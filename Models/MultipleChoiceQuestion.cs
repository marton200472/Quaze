namespace Quaze.Models;

public class MultipleChoiceQuestion : IQuestion
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeLimit { get; set; }
    public List<string> Choices { get; set; }
    public List<int> AnswerIndexs { get; set; }
}