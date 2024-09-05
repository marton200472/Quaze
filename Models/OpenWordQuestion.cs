namespace Quaze.Models;

public class OpenWordQuestion : IQuestion
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeLimit { get; set; }
    public string Answer { get; set; }
}