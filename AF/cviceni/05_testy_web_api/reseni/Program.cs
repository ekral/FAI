using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Studenti.WebAPI
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
    }

    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/students/", WebAPI.VratVsechnyStudenty);
            app.MapGet("/students/{id}", WebAPI.VratStudentaPodleId);
            app.MapPost("/students/", WebAPI.VlozNovehoStudenta);

            app.Run();
        }  
    }

    public static class WebAPI
    {
        // 1) Vytvorte a namapujte endpoit pro metodu VratVsechnyStudenty
        public static async Task<Ok<Student[]>> VratVsechnyStudenty(StudentContext context)
        {
            return TypedResults.Ok(await context.Studenti.ToArrayAsync());
        }
        // 2) Vytvorte a namapujte endpoit pro metodu VlozNovehoStudenta
        public static async Task<Created<Student>> VlozNovehoStudenta(Student student, StudentContext context)
        {
            context.Add(student);

            await context.SaveChangesAsync();

            return TypedResults.Created($"/Students/GetStudent/{student.StudentId}", student);
        }

        public static async Task<Results<Ok<Student>, NotFound>> VratStudentaPodleId(int id, StudentContext context)
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
    }
}
