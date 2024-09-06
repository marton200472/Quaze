using Quaze.Models;

namespace Quaze;

public class SessionService : BackgroundService
{
    public List<Session> Sessions { get; set; } = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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