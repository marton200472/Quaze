namespace Quaze.Models;

public class Session
{
    public string Id { get; set; }
    public List<Participant> Participants { get; set; }
}