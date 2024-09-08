namespace Quaze.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Question> Questions { get; set; } = new();
    public User Owner { get; set; }
    public string Description { get; set; }
    // Title image
}