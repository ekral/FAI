using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Studenti.WebAPI.Data;
using Studenti.WebAPI.Models;

namespace Studenti.WebAPI
{
    public record PaginatedResult(Student[] Students, int Total);

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
            app.MapGet("/students/page", GetStudentsPage);
            app.MapGet("/students/{id}", GetStudentById);
            app.MapPut("/students/{id}", UpdateStudent);

            app.Run();
        }

        public static async Task<Created> Seed(StudentContext context)
        {
            await context.Database.EnsureDeletedAsync();

            if (await context.Database.EnsureCreatedAsync())
            {
                string[] names = ["Jiri", "Karel", "Alena", "Petr", "Samuel", "Tereza", "Marko", "David", "Jan", "Filip", "Martin", "Erik", "Jana", "Ondrej"];

                for (int i = 0; i < 128; i++)
                {
                    string jmeno = names[Random.Shared.Next(names.Length - 1)];
                    bool studuje = Random.Shared.NextDouble() >= 0.5;

                    Student novy = new Student() { Jmeno = jmeno, Studuje = studuje };

                    await context.AddAsync(novy);
                }

                await context.SaveChangesAsync();
            }

            return TypedResults.Created();
        }

        public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
        {
            return TypedResults.Ok(await context.Studenti.ToArrayAsync());
        }

        public static async Task<Ok<PaginatedResult>> GetStudentsPage(StudentContext context, int startIndex, int count, string? sortBy = null, bool? descending = false)
        {
            IQueryable<Student> query = context.Studenti;

            if (descending == false)
            {
                query = sortBy switch
                {
                    "StudentId" => query.OrderBy(s => s.StudentId),
                    "Jmeno" => query.OrderBy(s => s.Jmeno),
                    "Studuje" => query.OrderBy(s => s.Studuje),
                    _ => query
                };
            }
            else
            {
                query = sortBy switch
                {
                    "StudentId" => query.OrderByDescending(s => s.StudentId),
                    "Jmeno" => query.OrderByDescending(s => s.Jmeno),
                    "Studuje" => query.OrderByDescending(s => s.Studuje),
                    _ => query
                };
            }

            query = query.Skip(startIndex).Take(count);

            Student[] students = await query.ToArrayAsync();
            int total = await context.Studenti.CountAsync();

            PaginatedResult result = new(students, total);

            return TypedResults.Ok(result);
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
