using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Students.WebAPI.Data;
using Students.WebAPI.Models;

namespace Students.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var studentItems = app.MapGroup("/Students");

            studentItems.MapPost("/Seed", WebApiVersion1.Seed);
            studentItems.MapGet("/GetAllStudents", WebApiVersion1.GetAllStudents);
            studentItems.MapGet("/GetActiveStudents", WebApiVersion1.GetActiveStudents);
            studentItems.MapGet("/GetStudent/{id}", WebApiVersion1.GetStudent);
            studentItems.MapPost("/", WebApiVersion1.CreateStudent);
            studentItems.MapPut("/{id}", WebApiVersion1.UpdateTodo);
            studentItems.MapDelete("/{id}", WebApiVersion1.DeleteStudent);

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
            Student? student = await context.Studenti.FindAsync(id);

            if(student is not null)
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
        
        public static async Task<Results<NotFound, NoContent>> UpdateTodo(int id, Student inputStudent, StudentContext context)
        {
            Student? student = await context.Studenti.FindAsync(id);

            if(student is null)
            {
                return TypedResults.NotFound();
            }

            student.Jmeno = inputStudent.Jmeno;
            student.Studuje = inputStudent.Studuje;

            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<IResult> DeleteStudent(int id, StudentContext context)
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
