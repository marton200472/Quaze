using Microsoft.EntityFrameworkCore;
using Quaze.Models;

namespace Quaze.Data;

public class QuazeDbContext : DbContext
{
    public DbSet<Quiz> Quizes { get; set; }

    public QuazeDbContext(DbContextOptions o) : base(o)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quiz>().Property(x=>x.Questions).HasConversion(x=>Newtonsoft.Json.JsonConvert.SerializeObject(x.Select(t=>new DbQuestion(t))),y=>Newtonsoft.Json.JsonConvert.DeserializeObject<List<DbQuestion>>(y)!.Select(FromDbQuestion).ToList());
    }

    private IQuestion FromDbQuestion(DbQuestion t) {
        if (t.QuestionType == DbQuestion.Type.Choice)
        {
            return new MultipleChoiceQuestion() {
                Title = t.Title,
                Description = t.Description,
                TimeLimit = t.TimeLimit,
                Choices = t.Choices!,
                AnswerIndexs = t.AnswerIndexes!
            };
        }
        else {
            return new OpenWordQuestion() {
                Title = t.Title,
                Description = t.Description,
                TimeLimit = t.TimeLimit,
                Answer = t.Answer!
            };
        }
    }
}