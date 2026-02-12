using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Studenti.WebAPI.Apis;
using Studenti.WebAPI.Data;
using Studenti.WebAPI.Models;

namespace Studenti.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

            builder.Services.AddCors(); // CORS 

            WebApplication app = builder.Build();

            app.UseCors(p => p.WithOrigins("https://localhost:7235").AllowCredentials().AllowAnyMethod().AllowAnyHeader()); // CORS 

            app.MapStudentApi();

            app.Run();
        }
    }
}
