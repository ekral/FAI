using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UTB.School.Contracts;
using UTB.School.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<SchoolContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();


app.MapPost("/dev/seed", Seed);
app.MapGet("/students", GetStudents);
app.MapGet("/students/{id:int}", GetStudent);
app.MapPost("/students", CreateStudent);
app.MapPut("/students/{id:int}", UpdateStudent);
app.MapDelete("/students/{id:int}", DeleteStudent);
app.MapPatch("/students/{id}", PatchStudentActivity);

app.Run();

static async Task<NoContent> Seed(SchoolContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    context.Students.AddRange(
        new Student { Name = "Jan", IsActive = true },
        new Student { Name = "Eva", IsActive = true },
        new Student { Name = "Petr", IsActive = false }
    );

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<Ok<StudentDto[]>> GetStudents(bool? isActive, SchoolContext context)
{
    var query = context.Students.AsQueryable();

    if (isActive.HasValue)
    {
        query = query.Where(s => s.IsActive == isActive);
    }

    StudentDto[] students = await query.Select(s => new StudentDto(s.Id, s.Name, s.IsActive))
                                       .ToArrayAsync();

    return TypedResults.Ok(students);
}

static async Task<Results<Ok<StudentDto>, NotFound>> GetStudent(int id, SchoolContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        return TypedResults.Ok(new StudentDto(student.Id, student.Name, student.IsActive));
    }
    else
    {
        return TypedResults.NotFound();
    }
}

static async Task<Created<StudentDto>> CreateStudent(StudentRequestDto request, SchoolContext context)
{
    var student = new Student { Name = request.Name, IsActive = request.IsActive };

    context.Add(student);

    await context.SaveChangesAsync();

    return TypedResults.Created($"/students/{student.Id}", new StudentDto(student.Id, student.Name, student.IsActive));
}

static async Task<Results<NoContent, NotFound>> UpdateStudent(int id, StudentRequestDto request, SchoolContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        student.Name = request.Name;
        student.IsActive = request.IsActive;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}

static async Task<Results<NoContent, NotFound>> DeleteStudent(int id, SchoolContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        context.Students.Remove(student);

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}

static async Task<Results<NoContent, NotFound>> PatchStudentActivity(int id, StudentPatchRequestDto request, SchoolContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        student.IsActive = request.IsActive;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}