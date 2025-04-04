using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Studenti.WebAPI.Data;
using Studenti.WebAPI.DTOs;
using Studenti.WebAPI.Models;

namespace Studenti.WebAPI.Apis
{
    public static class StudentApi
    {
        public static IEndpointRouteBuilder MapStudentApi(this IEndpointRouteBuilder app)
        {
            app.MapGet("/seed", Seed);
            app.MapGet("/students", GetAllStudents);
            app.MapGet("/students/page", GetStudentsPage);
            app.MapGet("/students/{id}", GetStudentById);
            app.MapPut("/students/{id}", UpdateStudent);

            return app;
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

        public static async Task<Ok<PaginationResult>> GetStudentsPage(StudentContext context, int startIndex, int count, string? sortBy = null, string? direction = null)
        {
            IQueryable<Student> query = context.Studenti;

            if (sortBy is not null && direction is not null)
            {
                switch (direction)
                {
                    case "Ascending":
                        query = sortBy switch
                        {
                            "StudentId" => query.OrderBy(s => s.StudentId),
                            "Jmeno" => query.OrderBy(s => s.Jmeno),
                            "Studuje" => query.OrderBy(s => s.Studuje),
                            _ => query
                        };
                        break;
                    case "Descending":
                        query = sortBy switch
                        {
                            "StudentId" => query.OrderByDescending(s => s.StudentId),
                            "Jmeno" => query.OrderByDescending(s => s.Jmeno),
                            "Studuje" => query.OrderByDescending(s => s.Studuje),
                            _ => query
                        };
                        break;
                }
            }

            Student[] students = await query.Skip(startIndex).Take(count).ToArrayAsync();
            int total = await context.Studenti.CountAsync();

            var result = new PaginationResult(students, total);

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
