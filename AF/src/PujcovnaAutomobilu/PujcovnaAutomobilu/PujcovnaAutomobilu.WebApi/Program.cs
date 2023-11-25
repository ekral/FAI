using Microsoft.AspNetCore.Http.HttpResults;
using PujcovnaAutomobilu.Models;
using PujcovnaAutomobilu.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddDbContext<PujcovnaAutomobiluContext>();

var app = builder.Build();

app.MapGet("/", IResult (PujcovnaAutomobiluContext context) => Results.Ok(context.Automobils)).Produces(400);

app.MapGet("/Automobil/{id}", async (int id, PujcovnaAutomobiluContext context) =>
{
    Automobil? automobil = await context.Automobils.FindAsync(id);

    if (automobil is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(automobil);
});

app.MapGet("/Pujcit/{id}", WebApiVersion1.VratAutomobil);

app.Run();

public static class WebApiVersion1
{
    public static async Task<Results<NotFound, BadRequest, Ok<Automobil>>> VratAutomobil(int id, IEmailSender emailSender, PujcovnaAutomobiluContext context)
    {
        Automobil? automobil = await context.Automobils.FindAsync(id);

        if(automobil is null)
        {
            return TypedResults.NotFound();
        }

        if (automobil.Pujceno)
        {
            return TypedResults.BadRequest();
        }

        automobil.Pujceno = true;

        int count = await context.SaveChangesAsync();

        emailSender.SendEmail();

        return TypedResults.Ok(automobil);
    }
}
