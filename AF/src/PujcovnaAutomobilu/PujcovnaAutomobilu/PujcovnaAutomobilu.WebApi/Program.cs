using PujcovnaAutomobilu.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PujcovnaAutomobiluContext>();

var app = builder.Build();

app.MapGet("/", (PujcovnaAutomobiluContext context) => context.Automobils);

app.MapGet("/Pujcit/{id}", async (int id, PujcovnaAutomobiluContext context) =>
{
    Automobil? automobil = await context.Automobils.FindAsync(id);

    if(automobil is null)
    {
        return Results.NotFound();
    }

    if(automobil.Pujceno)
    {
        return Results.BadRequest();
    }

    automobil.Pujceno = true;

    int count = await context.SaveChangesAsync();

    return Results.Ok();
});

app.Run();
