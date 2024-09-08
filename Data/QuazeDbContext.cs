using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quaze.Models;

namespace Quaze.Data;

public class QuazeDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Quiz> Quizes { get; set; }
    public DbSet<DbImage> Images { get; set; }

    public QuazeDbContext(DbContextOptions o) : base(o)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quiz>().Property(x=>x.Questions).HasConversion(x=>Newtonsoft.Json.JsonConvert.SerializeObject(x.Select(t=>new Question(t))),y=>Newtonsoft.Json.JsonConvert.DeserializeObject<List<Question>>(y)!.Select(FromDbQuestion).ToList());
        base.OnModelCreating(modelBuilder);
    }
}