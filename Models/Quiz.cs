namespace Quaze.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<IQuestion> Questions { get; set; }
    // Title image
}