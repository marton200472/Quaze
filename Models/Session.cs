namespace Quaze.Models;

public class Session
{
    public string Id { get; set; }
    public List<QuizUser> Participants { get; set; }
}