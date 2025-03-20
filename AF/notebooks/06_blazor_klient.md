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

##  Blazor WebAssembly

Nyní vytvoříme projekt "Blazor WebAssembly Standalone App" využívající render mód Interactive WebAssembly.

V projektu máme zaregistrovaného HttpClienta jako službu s lifetimem Scoped, pro každý request se tedy vytvoří nový HttpClient. Adresa "https://localhost:7042" představuje adresu našeho WebAPI.

```csharp
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7042") });
```



