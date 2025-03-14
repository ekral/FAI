using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebApplicationWebApi
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

            string? connectionString = builder.Configuration.GetConnectionString("Studenti");
            builder.Services.AddDbContext<StudentContext>(options => options.UseSqlite(connectionString));

            var app = builder.Build();

            app.MapGet("/seed", Seed);
            app.MapGet("/students", GetAllStudents);
            app.MapGet("/students/active", GetActiveStudents);
            app.MapGet("/students/{id}", GetStudentById);
            app.MapPost("/students", AddStudent);
            app.MapPut("/students/{id}", UpdateStudent);
            app.MapPatch("/students/{id}", FinishStudies);
            app.MapDelete("/students/{id}", DeleteStudent);
            

            app.Run();
        }

        public static async Task<Created> Seed(StudentContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            Student student1 = new() { StudentId = 1, Jmeno = "Matylda", Studuje = true };
            Student student2 = new() { StudentId = 2, Jmeno = "Petr", Studuje = false};
            Student student3 = new() { StudentId = 3, Jmeno = "Jiri", Studuje = true };

            await context.Studenti.AddRangeAsync(student1, student2, student3);

            await context.SaveChangesAsync();

            return TypedResults.Created();
        }

        public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
        {
            Student[] studenti = await context.Studenti.ToArrayAsync();

            return TypedResults.Ok(studenti);
        }

        public static async Task<Ok<Student[]>> GetActiveStudents(StudentContext context)
        {
            Student[] studenti = await context.Studenti.Where(s => s.Studuje).ToArrayAsync();

            return TypedResults.Ok(studenti);
        }

        public static async Task<Results<Ok<Student>, NotFound>> GetStudentById(int id, StudentContext context)
        {
            Student? student = await context.Studenti.FindAsync(id);

            if (student is not null)
            {
                return TypedResults.Ok(student);
            }

            return TypedResults.NotFound();
        }

        public static async Task<Created<Student>> AddStudent(Student student, StudentContext context)
        {
            await context.Studenti.AddAsync(student);

            await context.SaveChangesAsync();

            return TypedResults.Created($"students/{student.StudentId}", student);
        }

        public static async Task<Results<NoContent, NotFound>> UpdateStudent(int id, Student input, StudentContext context)
        {
            Student? student = await context.Studenti.FindAsync(id);

            if (student is not null)
            {
                student.Jmeno = input.Jmeno;
                student.Studuje = input.Studuje;

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        public static async Task<Results<NoContent, NotFound, BadRequest>> FinishStudies(int id, StudentContext context)
        {
            Student? student = await context.Studenti.FindAsync(id);

            if (student is not null)
            {
                if(!student.Studuje)
                {
                    return TypedResults.BadRequest();
                }

                student.Studuje = false;
             
                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        public static async Task<Results<NoContent, NotFound>> DeleteStudent(int id, StudentContext context)
        {
            Student? student = await context.Studenti.FindAsync(id);

            if (student is not null)
            {
                context.Studenti.Remove(student);

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
