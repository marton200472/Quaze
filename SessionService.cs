using Quaze.Models;

namespace Quaze;

public class SessionService : BackgroundService
{
    public List<Session> Sessions { get; set; } = new();
    public event EventHandler TimerElapsed;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            await Task.Delay(1000);
            TimerElapsed?.Invoke(this, EventArgs.Empty);
        }
    }
}