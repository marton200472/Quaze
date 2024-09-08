namespace Quaze.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public List<Question> Questions { get; set; } = new();
    // Title image
}