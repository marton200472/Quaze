using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quaze;
using Quaze.Components;
using Quaze.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

    builder.Services.AddRazorPages();
    

builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<IHostedService>(p => p.GetRequiredService<SessionService>());

string connStr;
if (builder.Environment.IsDevelopment())
{
    connStr = builder.Configuration.GetConnectionString("Local")!;
}
else {
    connStr = builder.Configuration.GetConnectionString("Default")!;
}

builder.Services.AddTransient(x=>new QuazeDbContext(new DbContextOptionsBuilder<QuazeDbContext>().UseMySql(connStr, ServerVersion.AutoDetect(connStr)).Options));

builder.Services.AddDbContextFactory<QuazeDbContext>(c=>{
    c.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();



builder.Services.AddIdentity<Quaze.Models.User,IdentityRole>(c=>{

})
.AddEntityFrameworkStores<QuazeDbContext>();

builder.Services.ConfigureApplicationCookie(c=>{
    c.LoginPath = "/auth/login";
    c.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

    app.MapRazorPages();


//image endpoint
app.MapGet("api/image/{id:guid}",async ([FromRoute] Guid id, [FromServices]QuazeDbContext db) => {
    var image = await db.Images.FindAsync(id);
    if (image is null)
    {
        return Results.NotFound();
    }
    return Results.File(image.Data);
});

app.MapGet("auth/logout", async ([FromServices]SignInManager<Quaze.Models.User> signInManager) => {
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
});

using (IServiceScope scope = app.Services.CreateScope())
    {
        var db = (QuazeDbContext)scope.ServiceProvider.GetRequiredService(typeof(QuazeDbContext));
        db.Database.Migrate();
    }

app.Run();
