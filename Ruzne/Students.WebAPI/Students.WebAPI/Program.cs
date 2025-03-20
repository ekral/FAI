using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Students.WebAPI.Apis;
using Students.WebAPI.Data;
using Students.WebAPI.Models;

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<StudentContext>();

//    if (context is not null && await context.Database.EnsureCreatedAsync())
//    {
//        await context.AddRangeAsync(
//            new Student() { Jmeno = "Jiri", Studuje = true },
//            new Student() { Jmeno = "Karel", Studuje = false },
//            new Student() { Jmeno = "Alena", Studuje = true });

//        await context.SaveChangesAsync();
//    }
//}

namespace Students.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

            builder.Services.AddCors(); // CORS 

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCors(p => p.WithOrigins("https://localhost:7215").AllowCredentials().AllowAnyMethod().AllowAnyHeader()); // CORS 

            app.MapStudentApi();

            app.Run();
        }
    }
}
