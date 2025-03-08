# 03 Minimal Web API

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu si probereme práci s webovými službami s pomocí Minimal Web API.

Web API (Web Application Programming Interface) je sada pravidel a protokolů umožňující komunikovat programům prostřednictvím internetu. REST (Representation State Transfer) je druh Web API a představuj architektonický styl použivající standartní HTTP metody (GET, POST, PUT a DELETE) zpřístpňující endpoity identifikvané pomocí URI. Pro přenos dat využívá přitom především format JSON.

[Minimal Web API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api) je zjednodušený způsob tvorby HTTP API pomocí ASP.NET Core.

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

`DbContext` potom použijeme ve web API s pomocí následujícího příkazu, který zaregistruje `StudentContext` do Inversion of Control containeru s lifetimem `Scoped`. Což znamená, že pro každý web request se nám vytvoří nová instance třídy `StudentContext`.

```csharp
builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));
```

Celý kód potom bude vypadat následovně:

```csharp
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Students.WebAPI.Data;
using Students.WebAPI.Models;

namespace Students.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

            var app = builder.Build();

            app.MapPost("/seed", WebApiVersion1.Seed);
            app.MapGet("/students/", WebApiVersion1.GetAllStudents);
            app.MapGet("/students/active", WebApiVersion1.GetActiveStudents);
            app.MapGet("/students/{id}", WebApiVersion1.GetStudent);
            app.MapPost("/students/", WebApiVersion1.CreateStudent);
            app.MapPut("/students/{id}", WebApiVersion1.UpdateStudent);
            app.MapDelete("/students/{id}", WebApiVersion1.DeleteStudent);

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
```

Předcházející metody můžeme ve Visual Studiu zavolat pomocí souboru s příponou `.http`. V JetBrains Rideru můžeme použít [plugin HTTP Client﻿](https://www.jetbrains.com/help/rider/Http_client_in__product__code_editor.html).

Obsah souboru vypadá následovně:

```json
@Students.WebAPI_HostAddress = https://localhost:7042

POST {{Students.WebAPI_HostAddress}}/Seed

###

GET {{Students.WebAPI_HostAddress}}/Students/

###

GET {{Students.WebAPI_HostAddress}}/Students/Active

###

GET {{Students.WebAPI_HostAddress}}/students/1

###

POST {{Students.WebAPI_HostAddress}}/Students
Content-Type: application/json

{
  "studentId": 0,
  "jmeno": "Lenka",
  "studuje": true
}

###

PUT {{Students.WebAPI_HostAddress}}/students/1
Content-Type: application/json

{
  "studentId": 1,
  "jmeno": "Novotna",
  "studuje": true
}
###

DELETE {{Students.WebAPI_HostAddress}}/students/1

###

```

## Group

Předcházející kód můžeme vylepšit, všimněte si, že v mapování se opakuje cesta "students". Abychom ji nemuseli pořád opakovat, tak můžeme využít metodu `MapGroup`:

```csharp
var studentItems = app.MapGroup("/students");

studentItems.MapGet("/", WebApiVersion1.GetAllStudents);
studentItems.MapGet("/active", WebApiVersion1.GetActiveStudents);
studentItems.MapGet("/{id}", WebApiVersion1.GetStudent);
studentItems.MapPost("/", WebApiVersion1.CreateStudent);
studentItems.MapPut("/{id}", WebApiVersion1.UpdateStudent);
studentItems.MapDelete("/{id}", WebApiVersion1.DeleteStudent);
```

## Extension metoda

Aby neměla metoda Main moc řádků a byla přehlednější, tak se často mapování endpointů přesouvá do [extension metody](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).

Následující kód definuje extension metodu:

```csharp
public static IEndpointRouteBuilder MapStudentsApi(this IEndpointRouteBuilder app)
{
    app.MapPost("/seed", Seed);

    var studentItems = app.MapGroup("/students");

    studentItems.MapGet("/", GetAllStudents);
    studentItems.MapGet("/active", GetActiveStudents);
    studentItems.MapGet("/{id}", GetStudent);
    studentItems.MapPost("/", CreateStudent);
    studentItems.MapPut("/{id}", UpdateStudent);
    studentItems.MapDelete("/{id}", DeleteStudent);

    return app;
}
```

A metoda Main bude vypadat následovně:

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

    WebApplication app = builder.Build();

    app.MapStudentsApi();

    app.Run();
}
```

## OpenAPI

[OpenApi](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview?view=aspnetcore-9.0) je standard pro dokumentaci HTTP aplikačních rozhraní nezávisle na programovacím jazyku.

Projekt musí mít referenci na nuget balíček [Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi). 

V metodě main potom přidáme označené řádky 

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    // Pridany radek
    builder.Services.AddOpenApi(); // Document name is v1

    builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

    WebApplication app = builder.Build();

    // Pridany blok
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.MapStudentsApi();

    app.Run();
}
```

Na adrese endpointu `https://localhost:<port>/openapi/v1.json` potom najdeme vygenerovanou dokumentaci. Název v1 je výchozí, mohli bychom ho změnit předáním stringového argumentu metodě AddOpenApi. Adresa by potom byla `https://localhost:<port>/openapi/nazev.json`.

 ```csharp
 builder.Services.AddOpenApi("nazev");
 ```

 Také můžeme pomocí atributů, nebo pomocí fluent api zadávat další metadata pro dokumentaci. Například id Endpointu zadáme pomocí fuent api metody `WithName`:

 ```csharp
studentItems.MapGet("/", GetAllStudents).WithName("GetAllStudents");
 ```


