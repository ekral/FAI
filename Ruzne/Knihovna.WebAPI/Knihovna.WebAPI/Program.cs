
using Knihovna.WebAPI.Apis;
using Knihovna.WebAPI.Data;
using Knihovna.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddCors();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            
            app.UseCors(p => p.WithOrigins("https://localhost:7074").AllowCredentials().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();


            app.MapPost("/seed", Seed);

            app.MapBookApis();

            app.Run();
        }

        public static async Task<Created> Seed(KnihovnaContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Knihy.AddRangeAsync(
                new Kniha() { KnihaId = 1, Nazev = "Svejk" },
                new Kniha() { KnihaId = 2, Nazev = "Temno" },
                new Kniha() { KnihaId = 3, Nazev = "Pan prstenu" }
                );

            await context.Ctenari.AddRangeAsync(
                new Ctenar() { CtenarId = 1, Jmeno = "Petr" },
                new Ctenar() { CtenarId = 2, Jmeno = "Erik" },
                new Ctenar() { CtenarId = 3, Jmeno = "Alena" }
                );

            await context.Vypujcky.AddRangeAsync(
                new Vypujcka() { VypujckaId = 1, KnihaId = 1, CtenarId = 1, DatumVypujcky = DateOnly.FromDateTime(DateTime.Now) }
                );

            await context.SaveChangesAsync();

            return TypedResults.Created();
        }

    }


}
