using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Students.WebAPI.Data;
using Students.WebAPI.DTOs;
using Students.WebAPI.Models;

namespace Students.WebAPI.Apis
{
    public static class StudentApi 
    {
        public static IEndpointRouteBuilder MapStudentApi(this IEndpointRouteBuilder app)
        {
            app.MapPost("/seed", Seed);

            var studentItems = app.MapGroup("/students");

            studentItems.MapGet("/", GetAllStudents).WithName("GetAllStudents");
            studentItems.MapGet("/active", GetActiveStudents);
            studentItems.MapGet("/page", GetStudentsPage);
            studentItems.MapGet("/{id}", GetStudent);
            studentItems.MapPost("/", CreateStudent);
            studentItems.MapPut("/{id}", UpdateStudent);
            studentItems.MapDelete("/{id}", DeleteStudent);
            studentItems.MapPatch("/{id}", FinishStudies);

            return app;
        }

        public static async Task<Created> Seed(StudentContext context)
        {
            await context.Database.EnsureDeletedAsync();

            if (await context.Database.EnsureCreatedAsync())
            {
                string[] jmena = ["Jiri", "Karel", "Tereza", "Alena", "Matej", "Marko", "Pavlina", "David", "Adam", "Radko", "Filip", "Michal", "Martin", "Erik", "Petr", "Tomas"];

                for (int i = 0; i < 100; i++)
                {
                    string jmeno = jmena[Random.Shared.Next(jmena.Length - 1)];
                    bool studuje = Random.Shared.NextDouble() >= 0.5;

                    await context.AddAsync(new Student() { Jmeno = jmeno, Studuje = studuje });
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

            if(sortBy is not null && direction is not null)
            {
                switch(direction)
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

        public static async Task<Ok<Student[]>> GetActiveStudents(StudentContext context)
        {
            return TypedResults.Ok(await context.Studenti.Where(s => s.Studuje).ToArrayAsync());
        }

        public static async Task<Results<Ok<Student>, NotFound>> GetStudent(int id, StudentContext context)
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

        public static async Task<Created<Student>> CreateStudent(Student student, StudentContext context)
        {
            context.Add(student);

            await context.SaveChangesAsync();

            return TypedResults.Created($"/Students/GetStudent/{student.StudentId}", student);
        }

        public static async Task<Results<NoContent, NotFound>> UpdateStudent(int id, Student inputStudent, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                student.Jmeno = inputStudent.Jmeno;
                student.Studuje = inputStudent.Studuje;

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        public static async Task<Results<NoContent, NotFound>> DeleteStudent(int id, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                context.Remove(student);

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        public static async Task<Results<NoContent, NotFound>> FinishStudies(int id, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                student.Studuje = false;

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
