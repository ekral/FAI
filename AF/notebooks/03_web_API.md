# 03 Minimal Web API

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu si probereme práci s webovými službami s pomocí Minimal Web API.

Web API (Web Application Programming Interface) je sada pravidel a protokolů umožňující komunikovat programům prostřednictvím internetu. REST (Representation State Transfer) je druh Web API a představuj architektonický styl použivající standartní HTTP metody (GET, POST, PUT a DELETE) zpřístpňující endpoity identifikvané pomocí URI. Pro přenos dat využívá přitom především format JSON.

Minimal Web API je zjednodušený způsob tvorby HTTP API pomocí ASP.NET Core.

https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0#typedresults-vs-results

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-9.0

https://localhost:7042/openapi/v1.json

Následující kód vrátí na metodu GET text "Hello World". Představuje nejjednodušší program v Minimal Web API.

```csharp
var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.MapGet("/", () => "Hello World.");

app.Run();
```

V dalším příkladu budeme mít web API, které bude představovat evidenci studentů. Pro práci s databází budeme používat Entity Framework a Sqlite databázi.

Nejprve si nadefinujeme třídu student:

```csharp
public class Student
{
    public int StudentId {get; set;}
    public required string Jmeno {get; set;}
    public required bool Studuje {get;set;}
}
```

Potom si nadefinuje DbContext:

```csharp
public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
{
    public DbSet<Student> Studenti { get; set; }
}
```

`DbContext` potom použijeme ve web API s pomocí následujícího příkazu, který zaregistruje `StudentContext` do Inversion of Control containeru s lifetimem Scoped. Což znamená, že pro každý web request se nám vytvoří nová instance třídy `StudentContext`.

```csharp
builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));
```

Celý kód potom bude vypadat následovně:

```csharp
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

        var app = builder.Build();

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
```