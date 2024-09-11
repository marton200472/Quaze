using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Quaze.Data;
using Quaze.Models;

namespace Quaze.Components.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username {get;set;} = "";
    [BindProperty]
    public string Password {get; set;} = "";


    private readonly IDbContextFactory<QuazeDbContext> dbFactory;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public IEnumerable<string> Errors {get;private set;} = Enumerable.Empty<string>();

    public LoginModel(IDbContextFactory<QuazeDbContext> dbFactory, UserManager<User> uManager, SignInManager<User> signManager)
    {
        this.dbFactory = dbFactory;
        userManager = uManager;
        signInManager = signManager;
    }

    public async Task<IActionResult> OnPostAsync([FromQuery]string returnUrl) {

        if (Username is null || Password is null)
        {
            Errors = ["Username or password was empty."];
            return Page();   
        }
        var user = await userManager.FindByNameAsync(Username);
        if (user is null || !(await signInManager.PasswordSignInAsync(user, Password,false, false)).Succeeded)
        {
            Errors = ["User not found."];
            return Page();
        }

        return LocalRedirect(string.IsNullOrWhiteSpace(returnUrl)?"/":returnUrl);
    }
}