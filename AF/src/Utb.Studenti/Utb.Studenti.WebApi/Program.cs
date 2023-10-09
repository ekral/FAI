using Microsoft.EntityFrameworkCore;
using Utb.Studenti.Models;

// Jen pro vyvoj
using (StudentContext context = new())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/studenti", async (StudentContext context) =>
{
    var studenti = await context.Studenti.ToListAsync();
    return studenti;
});

app.Run();