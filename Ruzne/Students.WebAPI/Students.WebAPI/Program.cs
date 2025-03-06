using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Students.WebAPI.Apis;
using Students.WebAPI.Data;
using Students.WebAPI.Models;

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<StudentContext>();

//    if (context is not null && await context.Database.EnsureCreatedAsync())
//    {
//        await context.AddRangeAsync(
//            new Student() { Jmeno = "Jiri", Studuje = true },
//            new Student() { Jmeno = "Karel", Studuje = false },
//            new Student() { Jmeno = "Alena", Studuje = true });

//        await context.SaveChangesAsync();
//    }
//}

namespace Students.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

            WebApplication app = builder.Build();

            app.MapStudentsApi();

            app.Run();
        }
    }

    public static class WebApiVersion1
    {
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
            if(await context.Studenti.FindAsync(id) is Student student)
            {
                context.Remove(student);

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
