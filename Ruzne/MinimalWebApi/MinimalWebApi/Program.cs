
using Microsoft.EntityFrameworkCore;
using Studenti.Models;

namespace MinimalWebApi
{
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite(Settings.GetConnectionString("database.db")));

            builder.Services.AddAuthorization();
            
            builder.Services.AddOpenApi();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", GetAllStudents);

            app.Run();
        }

        public static async Task<IResult> GetAllStudents(StudentContext context)
        {
            return TypedResults.Ok(await context.Studenti.ToArrayAsync());
        }
    }
}
