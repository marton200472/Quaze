namespace Quaze.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<IQuestion> Questions { get; set; } = new();
    // Title image
}