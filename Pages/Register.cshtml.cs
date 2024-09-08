using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Quaze.Data;
using Quaze.Models;

namespace Quaze.Components.Pages;

public class RegisterModel : PageModel
{
    [BindProperty]
    public string Username {get;set;} = "";
    [BindProperty]
    public string Password {get; set;} = "";


    private readonly IDbContextFactory<QuazeDbContext> dbFactory;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public RegisterModel(IDbContextFactory<QuazeDbContext> dbFactory, UserManager<User> uManager, SignInManager<User> signManager)
    {
        this.dbFactory = dbFactory;
        userManager = uManager;
        signInManager = signManager;
    }

    public async Task<IActionResult> OnPostAsync() {
        var user = new User() {UserName = Username};
        await userManager.CreateAsync(user, Password);
        await signInManager.SignInAsync(user, true);
        return Redirect("/");
    }
}