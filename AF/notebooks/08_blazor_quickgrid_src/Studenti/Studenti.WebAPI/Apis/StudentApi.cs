using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Studenti.WebAPI.Data;
using Studenti.WebAPI.Models;

namespace Studenti.WebAPI.Apis
{
    public static class StudentApi
    {
        public static IEndpointRouteBuilder MapStudentApi(this IEndpointRouteBuilder app)
        {
            app.MapGet("/seed", Seed);
            app.MapGet("/students", GetAllStudents);
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
