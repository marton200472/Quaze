using Microsoft.AspNetCore.Identity;

namespace Quaze.Models;


public class User : IdentityUser
{
    public List<Quiz> Quizzes { get; set; } = new();
}