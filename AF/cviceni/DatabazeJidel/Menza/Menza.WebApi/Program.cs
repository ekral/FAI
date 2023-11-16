using Menza.Data;
using Menza.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MenzaContext>();

var app = builder.Build();

app.MapMenzaApi();

app.Run();

public static class MenzaApi
{
    public static IEndpointRouteBuilder MapMenzaApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/jidla", (MenzaContext context) => context.Jidla);
        app.MapGet("/jidla/{id:int}", MenzaApi.GetJidloById);

        return app;
    }

    public static async Task<Ok<IEnumerable<Jidlo>>> GetAllJidla(MenzaContext context)
    {
        List<Jidlo> jidla = await context.Jidla.ToListAsync();

        return TypedResults.Ok(jidla.AsEnumerable());
    }

    public static async Task<Results<Ok<Jidlo>, NotFound, BadRequest>> GetJidloById(int id, MenzaContext context)
    {
        if(id <= 0)
        {
            return TypedResults.BadRequest();
        }

        Jidlo? jidlo = await context.Jidla.FindAsync(id);

        if(jidlo is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(jidlo);

    }

}