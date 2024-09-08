using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quaze.Models;

namespace Quaze.Data;

public class QuazeDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Quiz> Quizes { get; set; }
    public DbSet<DbImage> Images { get; set; }
    public DbSet<Question> Questions { get; set; }

    public QuazeDbContext(DbContextOptions o) : base(o)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}