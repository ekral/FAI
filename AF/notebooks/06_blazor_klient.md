# 05 Blazor klient

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu si probereme jak vytvořit jednoduchého [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) klienta pro naši webovou službu. Konkrétně použijme Interactive WebAssembly [Render Mode](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0), tedy Client-side rendering (CSR) s využitím Blazor WebAssembly.

## WebAPI

Nejprve si vytvoříme WebAPI, kde budeme mít následující entitu a DbContext:

```csharp
public class Student
{
    public int StudentId { get; set; }
    public required string Jmeno { get; set; }
    public required bool Studuje { get; set; }
}
```

```csharp
public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
{
    public DbSet<Student> Studenti { get; set; }
}
```

A dále máme namapovaný následující endpoint, který vrátí všechny studenty v databázi.

```csharp
app.MapGet("/", GetAllStudents).WithName("GetAllStudents");


public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
{
    return TypedResults.Ok(await context.Studenti.ToArrayAsync());
}
```

Co je ale velmi důležité, tak do konfigurace a middlewaru musíme přidat podporu CORS (Cross-origin resource sharing), kde adresa "https://localhost:7074" je adresa Blazor aplikace, kterou zadáme až tuto aplikaci vytvoříme. CORS musíme povolit proto, že Blazor a WebAPI budou běžet na jiném portu. To znamená že jde o jinou doménu a prohlížeč může blokovat dotaz z klienta na jinou doménu, než na které běží.

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("DataSource=studenti.db"));

    builder.Services.AddCors(); // CORS 

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseCors(p => p.WithOrigins("https://localhost:7074").AllowCredentials().AllowAnyMethod().AllowAnyHeader()); // CORS 

    app.MapGet("/", GetAllStudents).WithName("GetAllStudents");

    app.Run();
}
```

##  Blazor WebAssembly

Nyní vytvoříme projekt "Blazor WebAssembly Standalone App" využívající render mód Interactive WebAssembly.

V projektu máme zaregistrovaného HttpClienta jako službu s lifetimem Scoped, pro každý request se tedy vytvoří nový HttpClient. Adresa "https://localhost:7042" představuje adresu našeho WebAPI.

```csharp
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7042") });
```

Do projektu si přidáme novou Razor Componentu Studenti.razor

```razor
@page "/students"

<PageTitle>Studenti</PageTitle>

<h3>Studenti</h3>

@if(studenti is not null)
{
    <table class="table">
        <thead>
            <tr>
                <th>Id</th>
                <th>Jmeno</th>
                <th>Studuje</th>
            </tr>
        </thead>
        <tbody>
            @foreach(Student student in studenti)
            {
                <tr>
                    <td>@student.StudentId</td>
                    <td>@student.Jmeno</td>
                    <td>@student.Studuje</td>
                </tr>
            }
        </tbody>
    </table>
}
```

A v kódu na pozadí načteme studenty:

```cs
using System.Net.Http.Json;

namespace Students.BlazorApp.Pages
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public required bool Studuje { get; set; }
    }

    public partial class Studenti(HttpClient client)
    {
        private readonly HttpClient client = client;
        private Student[]? studenti;

        protected override async Task OnInitializedAsync()
        {
            studenti = await client.GetFromJsonAsync<Student[]>("/students");
        }
    }
}
```