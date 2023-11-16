using Menza.Data;
using Menza.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MenzaContext>();

var app = builder.Build();

app.MapGet("/", (MenzaContext context) => context.Jidla);

// 🚀 Get ktery vrati jidlo podle Id
app.MapGet("/{id}", async (int id,MenzaContext context) =>
    await context.Jidla.FindAsync(id)
        is Jidlo jidlo
            ? Results.Ok(jidlo)
            : Results.NotFound());
app.Run();
