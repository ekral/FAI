
using Knihovna.WebAPI.Data;
using Knihovna.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Knihovna.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<KnihovnaContext>(opt => opt.UseSqlite("DataSource=knihovna.db"));
            
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.RegisterEndpoints();

            app.Run();
        }
    }

    static class EndpointDefinitions
    {
        public static void RegisterEndpoints(this WebApplication app)
        {
            app.MapPost("/Books/Seed", WebApiVersion1.Seed);
            app.MapGet("/Books/GetAllBooks", WebApiVersion1.GetAllBooks);
        }
    }

    static class WebApiVersion1
    {
        public static async Task<Created> Seed(KnihovnaContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Knihy.AddRangeAsync(
                new Kniha() { KnihaId = 1, Nazev = "Svejk"},
                new Kniha() { KnihaId = 2, Nazev = "Temno"},
                new Kniha() { KnihaId = 3, Nazev = "Pan prstenu"}
                );

            await context.Ctenari.AddRangeAsync(
                new Ctenar() { CtenarId = 1, Jmeno = "Petr"},
                new Ctenar() { CtenarId = 2, Jmeno = "Erik"},
                new Ctenar() { CtenarId = 3, Jmeno = "Alena"}
                );

            await context.Vypujcky.AddRangeAsync(
                new Vypujcka() { VypujckaId = 1, KnihaId = 1, CtenarId = 1, DatumVypujcky = DateOnly.FromDateTime(DateTime.Now)}
                );
           
            await context.SaveChangesAsync();

            return TypedResults.Created();
        }

        public static async Task<Ok<Kniha[]>> GetAllBooks(KnihovnaContext context)
        {
            return TypedResults.Ok(await context.Knihy.ToArrayAsync());
        }
    }
}
