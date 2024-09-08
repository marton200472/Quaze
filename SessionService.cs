using Quaze.Models;

namespace Quaze;

public class SessionService : BackgroundService
{
    public List<Session> Sessions { get; set; } = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Sessions.Add(new Session("1", new Quiz() {Title="Test quiz", Questions = new List<Question>(){ new Question() { Title = "Teszt", Description = "Teszt", TimeLimit = 20 } }}));
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