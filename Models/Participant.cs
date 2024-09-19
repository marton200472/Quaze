namespace Quaze.Models;

public class Participant
{
    public string Name { get; set; }
    public Dictionary<int, List<string>> Answers { get; set; } = new();
    public int Points { get; set; } = 0;
}
