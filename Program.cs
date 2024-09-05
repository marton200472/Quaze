using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quaze;
using Quaze.Components;
using Quaze.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<IHostedService>(p => p.GetRequiredService<SessionService>());


string connStr = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<QuazeDbContext>(c=>{
    c.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//image endpoint
app.MapGet("api/image/{id:guid}",([FromRoute] Guid id, [FromServices]QuazeDbContext db)=> id.ToString());

using (IServiceScope scope = app.Services.CreateScope())
    {
        var db = (QuazeDbContext)scope.ServiceProvider.GetRequiredService(typeof(QuazeDbContext));
        db.Database.Migrate();
    }

app.Run();
