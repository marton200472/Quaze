using Microsoft.EntityFrameworkCore;
using Quaze.Data;
using Quaze.Models;
using System.Security.Cryptography;

namespace Quaze;

public class SessionService : BackgroundService
{
    public List<Session> Sessions { get; set; } = new();

    private readonly IDbContextFactory<QuazeDbContext> dbFactory;

    public SessionService(IDbContextFactory<QuazeDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public async Task<string> StartNewSessionAsync(Quiz quiz, User user) {
        string id = "";
        do
        {
            id=RandomNumberGenerator.GetHexString(5);
        } while (Sessions.Any(x=>x.Id == id));
        using var db = await dbFactory.CreateDbContextAsync();
        quiz = await db.Quizes.Include(x=>x.Questions).FirstAsync(x=>x.Id == quiz.Id);
        var session = new Session(id, user.Id, quiz);
        Sessions.Add(session);
        return id;
    }

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