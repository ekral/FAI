using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MojeWebApplication
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public required bool Studuje { get; set; }
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

            string? connectionString = builder.Configuration.GetConnectionString("Students");
            builder.Services.AddDbContext<StudentContext>(options => options.UseSqlite(connectionString));

            var app = builder.Build();

            app.MapGet("/seed", Seed);
            app.MapGet("/students", GetAllStudents);
            app.MapGet("/students/studying", GetStudyingStudents);
            app.MapGet("/students/{id}", GetStudentById);
            app.MapPost("/students", AddStudent);
            app.MapPut("/students/{id}", ChangeStudent);
            app.MapPatch("/students/{id}", EndStudies);
            app.MapDelete ("/students/{id}", RemoveStudent);

            app.Run();
        }
        // Vrate jen studenty, kteri maji hodnotu property Studuje true
        public static async Task<Created> Seed(StudentContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            Student student1 = new() { StudentId = 1, Jmeno = "Marko", Studuje = true };
            Student student2 = new() { StudentId = 2, Jmeno = "Jakub", Studuje = false };
            Student student3 = new() { StudentId = 3, Jmeno = "Petr", Studuje = true };

            await context.Studenti.AddRangeAsync(student1, student2, student3);

            await context.SaveChangesAsync();

            return TypedResults.Created();
        }

        public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
        {
            Student[] studenti = await context.Studenti.ToArrayAsync();

            return TypedResults.Ok(studenti);
        }

        public static async Task<Ok<Student[]>> GetStudyingStudents(StudentContext context)
        {
            Student[] studenti = await context.Studenti
                .Where(s => s.Studuje)
                .ToArrayAsync();

            return TypedResults.Ok(studenti);
        }

        public static async Task<Results<Ok<Student>, NotFound>> GetStudentById(int id, StudentContext context)
        {
            if(await context.Studenti.FindAsync(id) is Student student)
            {
                return TypedResults.Ok(student);
            }

            return TypedResults.NotFound();
        }

        public static async Task<Created<Student>> AddStudent(Student student, StudentContext context)
        {
            await context.Studenti.AddAsync(student);
            await context.SaveChangesAsync();

            return TypedResults.Created($"/students/{student.StudentId}", student);
        }

        public static async Task<Results<NoContent, NotFound>> ChangeStudent(int id, Student input, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                student.Jmeno = input.Jmeno;
                student.Studuje = input.Studuje;

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        // napiste metodu, ktera ukonci studium studentovi se zadanym id
        // namapujte ji na Patch

        public static async Task<Results<NoContent, NotFound>> EndStudies(int id, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                student.Studuje = false;

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        public static async Task<Results<NoContent, NotFound>> RemoveStudent(int id, StudentContext context)
        {
            if (await context.Studenti.FindAsync(id) is Student student)
            {
                context.Studenti.Remove(student);

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

    }
}
