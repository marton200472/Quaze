using Quaze.Models;

namespace Quaze;

public class SessionService : BackgroundService
{
    public List<Session> Sessions { get; set; } = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Sessions.Add(new Session("1", new Quiz() {Title="Test quiz", Questions = new List<IQuestion>(){new MultipleChoiceQuestion() {Title="1. Kérdés", Description="1. kérdés leírása", TimeLimit = 10}}}));
        while (true)
        {
            await Task.Delay(1000);
            foreach (var s in Sessions)
            {
                s.OnTimerTick();
            }
        }
    }
}