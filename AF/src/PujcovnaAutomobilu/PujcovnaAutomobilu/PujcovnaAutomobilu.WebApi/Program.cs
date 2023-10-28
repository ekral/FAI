using PujcovnaAutomobilu.Models;
using PujcovnaAutomobilu.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddDbContext<PujcovnaAutomobiluContext>();

var app = builder.Build();

app.MapGet("/", (PujcovnaAutomobiluContext context) => context.Automobils);
app.MapGet("/Automobil/{id}", (PujcovnaAutomobiluContext context) => context.Automobils);

app.MapGet("/Pujcit/{id}", async (int id, IEmailSender emailSender, PujcovnaAutomobiluContext context) =>
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

    emailSender.SendEmail();

    return Results.Ok();
});

app.Run();
