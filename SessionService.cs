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

    public async Task<string> StartNewSessionAsync(Quiz quiz) {
        string id = "";
        do
        {
            id=RandomNumberGenerator.GetHexString(5);
        } while (Sessions.Any(x=>x.Id == id));
        using var db = await dbFactory.CreateDbContextAsync();
        quiz = await db.Quizes.Include(x=>x.Questions).FirstAsync(x=>x.Id == quiz.Id);
        var session = new Session(id, quiz);
        Sessions.Add(session);
        return id;
    }

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