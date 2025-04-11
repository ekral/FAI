using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Students.WebApi.Data;
using Students.WebApi.Models;

namespace Students.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));
            builder.Services.AddCors();
            var app = builder.Build();

            app.UseCors(p => p.WithOrigins("https://localhost:7193")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.MapGet("seed", WebApi.Seed);
            app.MapGet("students", WebApi.GetAllStudents);
            app.MapGet("students/{id}", WebApi.GetStudentById);

            app.Run();
        }
    }

    public static class WebApi
    {
        public static async Task<Created> Seed(StudentContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            Student s1 = new() { Jmeno = "Izabela", Studuje = true };
            Student s2 = new() { Jmeno = "Julius", Studuje = true };
            Student s3 = new() { Jmeno = "Karel", Studuje = true };

            await context.AddRangeAsync(s1, s2, s3);

            await context.SaveChangesAsync();

            return TypedResults.Created();
        }

        public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
        {
            Student[] studenti = await context.Studenti.ToArrayAsync();

            return TypedResults.Ok(studenti);
        }

        public static async Task<Results<Ok<Student>, NotFound>> GetStudentById(StudentContext context, int id)
        {
            Student? student = await context.Studenti.FindAsync(id);

            if (student is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(student);
        }
    }
}
