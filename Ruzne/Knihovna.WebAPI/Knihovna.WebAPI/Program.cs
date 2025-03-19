
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


            app.MapPost("/seed", WebApiVersion1.Seed);

            app.MapBookApis();

            app.Run();
        }
    }

    public static class WebApiVersion1
    {
        
            
    }
}
