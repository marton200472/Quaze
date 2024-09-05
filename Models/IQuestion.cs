namespace Quaze.Models;

public interface IQuestion
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeLimit { get; set; } // In seconds
    // Helper images
}