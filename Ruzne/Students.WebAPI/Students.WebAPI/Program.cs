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

            app.MapGet("/", WebApiVersion1.GetAllStudents);

            app.Run();
        }
    }

    public static class WebApiVersion1
    {
        public static async Task<IResult> Init(StudentContext context)
        {
            if (await context.Database.EnsureCreatedAsync())
            {
                await context.AddRangeAsync(
                    new Student() { Jmeno = "Jiri", Studuje = true },
                    new Student() { Jmeno = "Karel", Studuje = false },
                    new Student() { Jmeno = "Alena", Studuje = true });

                await context.SaveChangesAsync();
            }

            return TypedResults.NoContent();
        }

        public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
        {
            return TypedResults.Ok(await context.Studenti.ToArrayAsync());
        }
    }
}
