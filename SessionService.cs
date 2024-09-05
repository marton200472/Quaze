using Quaze.Models;

namespace Quaze;

public class SessionService : BackgroundService
{
    public List<Session> Sessions { get; set; } = new();
    public event EventHandler TimerTick;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            await Task.Delay(1000);
            TimerTick?.Invoke(this, EventArgs.Empty);
        }
    }
}