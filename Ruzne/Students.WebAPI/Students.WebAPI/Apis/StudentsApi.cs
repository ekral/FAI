using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Students.WebAPI.Data;
using Students.WebAPI.Models;

namespace Students.WebAPI.Apis
{
    public static class StudentsApi 
    {
        public static IEndpointRouteBuilder MapStudentsApi(this IEndpointRouteBuilder app)
        {
            app.MapPost("/seed", Seed);

            var studentItems = app.MapGroup("/students");

            studentItems.MapGet("/", GetAllStudents).WithName("GetAllStudents");
            studentItems.MapGet("/active", GetActiveStudents);
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
