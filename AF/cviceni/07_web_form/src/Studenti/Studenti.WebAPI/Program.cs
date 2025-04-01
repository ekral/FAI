using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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

            app.MapGet("/seed", Seed);
            app.MapGet("/students", GetAllStudents);
            app.MapGet("/students/{id}", GetStudentById);
            app.MapPut("/students/{id}", UpdateStudent);

            app.Run();
        }

        public static async Task<Created> Seed(StudentContext context)
        {
            await context.Database.EnsureDeletedAsync();

            if (await context.Database.EnsureCreatedAsync())
            {
                await context.AddRangeAsync(
                    new Student() { Jmeno = "Jiri", Studuje = true },
                    new Student() { Jmeno = "Karel", Studuje = false },
                    new Student() { Jmeno = "Alena", Studuje = true });

                await context.SaveChangesAsync();
            }

            return TypedResults.Created();
        }

        public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
        {
            return TypedResults.Ok(await context.Studenti.ToArrayAsync());
        }


        public static async Task<Results<Ok<Student>, NotFound>> GetStudentById(int id, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                return TypedResults.Ok(student);
            }
            else
            {
                return TypedResults.NotFound();
            }
        }

        public static async Task<Results<NoContent, NotFound>> UpdateStudent(int id, Student input, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                student.Jmeno = input.Jmeno;
                student.Studuje = input.Studuje;
        
                await context.SaveChangesAsync();
        
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }
        }
    }
}
