# 04 Testování Web API s Aspire – studijní materiál (bakalářské studium)

**autor: Erik Král ekral@utb.cz**
S asistencí: GitHub Copilot (GPT-5.3-Codex)

## 🎯 Definice

- Testování Web API ověřuje, že endpointy vrací správné HTTP status kódy, JSON data a správně pracují s databází.
- Integrační test testuje více vrstev aplikace najednou, například HTTP endpoint, EF Core i databázi.
- [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) je platforma pro .NET aplikace, která umí spouštět více projektů a jejich závislosti dohromady.
- V integračních testech nám Aspire pomáhá spustit skutečné Web API i databázi v prostředí blízkém reálnému provozu.

Použité technologie:

- `ASP.NET Core Minimal API`
- `Entity Framework Core`
- `PostgreSQL`
- `xUnit`
- `Aspire.Hosting.Testing`

---

## Proč nestačí jen unit testy

Unit test ověřuje malý izolovaný kus kódu, například jednu metodu. To je užitečné, ale u Web API často potřebujeme vědět i to, že:

- endpoint je opravdu namapovaný na správnou URL,
- data z requestu se správně deserializují,
- databáze je dostupná,
- EF Core správně ukládá nebo čte data,
- aplikace vrací správný HTTP status code.

Právě to řeší integrační testy.

---

## Projekt UTB.School

Budeme vycházet z projektu `UTB.School`, který obsahuje několik částí:

- `UTB.School.WebApi` – vlastní Minimal API
- `UTB.School.Db` – entity a `DbContext`
- `UTB.School.Contracts` – DTO recordy
- `UTB.School.AppHost` – Aspire projekt, který spouští infrastrukturu
- `UTB.School.Tests` – integrační testy

Tato architektura je vhodná proto, že odděluje API, databázový model, přenosové objekty a testy.

---

## 1. Datový model

V projektu je použita jednoduchá entita `Student`:

```csharp
namespace UTB.School.Db
{
    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required bool IsActive { get; set; }
    }
}
```

Databázový kontext obsahuje tabulku studentů:

```csharp
namespace UTB.School.Db
{
    public class SchoolContext(DbContextOptions<SchoolContext> options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
    }
}
```

---

## 2. DTO objekty

Stejně jako v předchozím materiálu nepoužíváme entitu přímo pro komunikaci s klientem, ale DTO objekty.

```csharp
namespace UTB.School.Contracts
{
    public record StudentDto(int Id, string Name, bool IsActive);
    public record StudentRequestDto(string Name, bool IsActive);
    public record StudentPatchRequestDto(bool IsActive);
}
```

Význam jednotlivých DTO:

- `StudentDto` vracíme klientovi
- `StudentRequestDto` používáme pro `POST` a `PUT`
- `StudentPatchRequestDto` používáme pro `PATCH`, kdy měníme jen část dat

---

## 3. Endpointy ve Web API

V `Program.cs` jsou namapované endpointy pro CRUD operace nad studenty:

```csharp
app.MapPost("/dev/seed", Seed);
app.MapGet("/students", GetStudents);
app.MapGet("/students/{id:int}", GetStudent);
app.MapPost("/students", CreateStudent);
app.MapPut("/students/{id:int}", UpdateStudent);
app.MapDelete("/students/{id:int}", DeleteStudent);
app.MapPatch("/students/{id}", PatchStudentActivity);
```

Například načtení studentů s volitelným filtrem `isActive` vypadá takto:

```csharp
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
```

Endpoint pro seed vytvoří testovací databázi a vloží tři studenty:

```csharp
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
```

---

## 4. Co do testů přináší Aspire

Aspire umí během testů spustit:

- databázový server,
- samotné Web API,
- propojení mezi nimi,
- health checks a čekání na připravenost služeb.

To znamená, že test nevolá jen metodu v paměti, ale komunikuje s opravdovou HTTP aplikací a opravdovou databází.

---

## 5. Aspire AppHost

Aspire infrastruktura je definovaná v projektu `UTB.School.AppHost`.

```csharp
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("postgres-testing-UTB.School");
}
else
{
    postgres = builder.AddPostgres("postgres")
                     .WithContainerName("postgres-UTB.School")
                     .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);
}

var database = postgres.AddDatabase("database");

builder.AddProject<Projects.UTB_School_WebApi>("webapi")
       .WithReference(database)
       .WaitFor(database);

builder.Build().Run();
```

Důležité body:

- v režimu `Testing` se používá testovací PostgreSQL kontejner,
- databáze je v Aspire zaregistrovaná pod názvem `database`,
- Web API dostane databázi přes `WithReference(database)`,
- `WaitFor(database)` zajistí, že se API spustí až po databázi.

---

## 6. Registrace databáze ve Web API

Ve Web API je databázový kontext zaregistrovaný takto:

```csharp
builder.AddNpgsqlDbContext<SchoolContext>("database");
```

Tento zápis říká, že `SchoolContext` má použít connection string ze zdroje `database`, který dodá Aspire AppHost.

To je zásadní rozdíl oproti jednoduchému příkladu se SQLite, kde jsme connection string zapisovali ručně přímo do `Program.cs`.

---

## 7. TestFixture

Základ integračních testů tvoří třída `TestFixture`. Ta připraví celé testovací prostředí.

```csharp
using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using UTB.School.Db;

public class TestFixture : IAsyncLifetime
{
    private DistributedApplication app = null!;
    private string? connectionString;
    public HttpClient HttpClient { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.UTB_School_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

        app = await builder.BuildAsync(TestContext.Current.CancellationToken);

        await app.StartAsync(TestContext.Current.CancellationToken);

        await app.ResourceNotifications.WaitForResourceHealthyAsync("database", TestContext.Current.CancellationToken);
        await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);

        connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);
        HttpClient = app.CreateHttpClient("webapi", "https");

        using var context = CreateContext();

        await context.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

        context.Students.AddRange(
            new Student { Name = "Jan", IsActive = true },
            new Student { Name = "Eva", IsActive = true },
            new Student { Name = "Petr", IsActive = false }
        );

        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    public SchoolContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new SchoolContext(options);
    }

    public async ValueTask DisposeAsync()
    {
        HttpClient.Dispose();
        await app.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
```

Co třída dělá:

1. Spustí celý Aspire AppHost v režimu `Testing`.
2. Počká, až budou `database` a `webapi` ve stavu healthy.
3. Vytvoří `HttpClient`, kterým budeme volat endpointy.
4. Získá connection string do databáze.
5. Připraví databázi a vloží seed data pro testy.

---

## 8. Sdílení fixture mezi testy

Aby se testovací prostředí nemuselo startovat pro každý test zvlášť, používá se kolekce:

```csharp
[CollectionDefinition("Database collection", DisableParallelization = true)]
public class DatabaseCollection : ICollectionFixture<TestFixture>
{
}
```

Potom konkrétní testovací třída používá stejnou fixture:

```csharp
[Collection("Database collection")]
public class StudentTests(TestFixture fixture)
{
    private readonly TestFixture fixture = fixture;
}
```

`DisableParallelization = true` je zde důležité, protože všechny testy pracují nad stejnou databází. Kdyby běžely paralelně, mohly by se navzájem ovlivňovat.

---

## 9. Příklad integračních testů

### POST `/students`

Tento test ověřuje vytvoření studenta, návratový status code `201 Created`, správný obsah DTO i skutečné uložení do databáze.

```csharp
[Fact]
public async Task CreateStudent_ReturnsCreatedAndPersistsStudent()
{
    var studentRequestDto = new StudentRequestDto("Franz Kafka", true);

    var response = await fixture.HttpClient.PostAsJsonAsync(
        "/students",
        studentRequestDto,
        TestContext.Current.CancellationToken);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    StudentDto? studentDto = await response.Content
        .ReadFromJsonAsync<StudentDto>(TestContext.Current.CancellationToken);

    Assert.NotNull(studentDto);
    Assert.Equal(studentRequestDto.Name, studentDto.Name);
    Assert.True(studentDto.IsActive);
    Assert.NotNull(response.Headers.Location);
    Assert.EndsWith($"/students/{studentDto.Id}", response.Headers.Location.ToString());

    using var context = fixture.CreateContext();

    Student? student = await context.Students.FindAsync(studentDto.Id, TestContext.Current.CancellationToken);

    Assert.NotNull(student);
    Assert.Equal(studentRequestDto.Name, student.Name);
    Assert.Equal(studentRequestDto.IsActive, student.IsActive);
}
```

### GET `/students`

Tento test ověřuje, že endpoint vrátí seed data vložená při inicializaci fixture.

```csharp
[Fact]
public async Task GetStudents_ReturnsAllSeededStudents()
{
    var response = await fixture.HttpClient.GetAsync("/students", TestContext.Current.CancellationToken);

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    StudentDto[]? students = await response.Content
        .ReadFromJsonAsync<StudentDto[]>(TestContext.Current.CancellationToken);

    Assert.NotNull(students);
    Assert.True(students.Length >= 3);
    Assert.Contains(students, s => s.Name == "Jan" && s.IsActive);
    Assert.Contains(students, s => s.Name == "Eva" && s.IsActive);
    Assert.Contains(students, s => s.Name == "Petr" && !s.IsActive);
}
```

### GET `/students/{id}` pro neexistující záznam

```csharp
[Fact]
public async Task GetStudent_ReturnsNotFound_WhenStudentDoesNotExist()
{
    var response = await fixture.HttpClient.GetAsync("/students/999", TestContext.Current.CancellationToken);

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
}
```

### DELETE `/students/{id}`

Tento test nejdřív vloží záznam přímo do databáze, potom ho smaže přes HTTP endpoint a nakonec ověří, že záznam už v databázi neexistuje.

```csharp
[Fact]
public async Task DeleteStudent_DeletesStudent_WhenExists()
{
    var tereza = new Student { Name = "Tereza", IsActive = true };

    using (var context = fixture.CreateContext())
    {
        context.Students.Add(tereza);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    var response = await fixture.HttpClient.DeleteAsync($"/students/{tereza.Id}", TestContext.Current.CancellationToken);

    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

    using var verificationContext = fixture.CreateContext();

    var deletedStudent = await verificationContext.Students.FindAsync(tereza.Id, TestContext.Current.CancellationToken);

    Assert.Null(deletedStudent);
}
```

---

## 10. Jak tyto testy číst

Každý integrační test má většinou tři části:

1. `Arrange` – připravíme data a prostředí.
2. `Act` – zavoláme endpoint přes `HttpClient`.
3. `Assert` – ověříme HTTP odpověď a případně i obsah databáze.

Typický postup:

- pošlu HTTP požadavek,
- zkontroluji `StatusCode`,
- deserializuji JSON odpověď na DTO,
- ověřím, že se skutečně změnila databáze.

---

## 11. Proč v testu kontrolovat i databázi

Samotná HTTP odpověď nestačí. Endpoint může vrátit správný JSON, ale data se do databáze vůbec nemusela uložit.

Proto je v integračních testech běžné kombinovat:

- kontrolu HTTP odpovědi,
- kontrolu stavu databáze přes nový `DbContext`.

Použití nového contextu je důležité, protože nechceme číst data z cache již existující instance EF Core.

---

## 12. Jak testy spustit

Předpoklady:

- nainstalovaný `.NET SDK`,
- spuštěný Docker Desktop nebo jiný kontejnerový runtime podporovaný Aspire.

Testy lze spustit příkazem:

```powershell
dotnet test
```

Při spuštění Aspire vytvoří testovací PostgreSQL kontejner, spustí Web API a po dokončení testů vše ukončí.

> Před spuštěním testů v příkazové řádce nebo v Rideru bude možná nutné nainstalovat https certifikáty  příkazem [Aspire CLI](https://aspire.dev/app-host/certificate-configuration/#using-the-aspire-cli-recommended):
> ```powershell
> aspire certs trust
> ```

---

## 13. Nejčastější chyby

### Test padá ještě před voláním API

Často neběží Docker nebo se nepodařilo spustit databázový kontejner.

### API vrací chybu připojení k databázi

V AppHostu nebo ve Web API nesouhlasí název resource. Pokud AppHost registruje databázi jako `database`, musí stejný název použít i `AddNpgsqlDbContext<SchoolContext>("database")`.

### Testy se navzájem ovlivňují

Typicky je problém v tom, že více testů mění stejnou databázi paralelně. Pomáhá použití jedné kolekce testů s vypnutou paralelizací.

### Test vrací jiná data, než očekáváme

Je potřeba si uvědomit, jestli test používá seed data z fixture, nebo si data připravuje sám.

---

## 14. Shrnutí

Aspire umožňuje psát velmi kvalitní integrační testy, protože:

- spouští skutečnou infrastrukturu,
- propojí Web API s databází,
- umožní volat endpointy přes `HttpClient`,
- dovolí ověřit reálné chování celé aplikace.

Na tomto příkladu nevidíme jen izolované metody, ale celý tok aplikace od HTTP požadavku až po databázi.

---

## ❓ Kontrolní otázky

1. Jaký je rozdíl mezi unit testem a integračním testem?
2. K čemu slouží `DistributedApplicationTestingBuilder`?
3. Proč v testech čekáme na `WaitForResourceHealthyAsync("database")`?
4. Proč testy používají `HttpClient` místo přímého volání metod z `Program.cs`?
5. Proč je vhodné po HTTP volání ověřovat i skutečný stav databáze?
6. K čemu slouží `DisableParallelization = true`?
7. Jakou roli má v Aspire AppHostu `WithReference(database)`?

---

## Úkol – Public Library s testy v Aspire

Vytvořte vlastní řešení na téma **Public Library**. Cílem je procvičit nejen Minimal API, ale hlavně integrační testy s Aspire.

### Entita

Použijte entitu `Book`:

```csharp
public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public bool IsArchived { get; set; }
}
```

### DTO

Definujte DTO pomocí `record`:

- `BookDto`
- `BookRequestDto`
- `BookPatchRequestDto`

### Endpointy

Implementujte tyto endpointy:

- `POST /dev/seed` smaže databázi, znovu ji vytvoří a vloží alespoň tři knihy
- `GET /books` vrátí všechny knihy
- `GET /books?isArchived=true` umožní filtrovat knihy podle archivace
- `GET /books/{id}` vrátí jednu knihu podle `Id`
- `POST /books` vytvoří novou knihu
- `PUT /books/{id}` nahradí existující knihu
- `PATCH /books/{id}` změní pouze `IsArchived`
- `DELETE /books/{id}` odstraní knihu

### Aspire část

Vytvořte:

- `PublicLibrary.WebApi`
- `PublicLibrary.Db`
- `PublicLibrary.Contracts`
- `PublicLibrary.AppHost`
- `PublicLibrary.Tests`

Použijte PostgreSQL v Aspire AppHostu a napojte Web API na databázi přes `WithReference(database)`.

### Testy

Napište integrační testy alespoň pro tyto scénáře:

1. `POST /books` vrátí `201 Created` a kniha se opravdu uloží do databáze.
2. `GET /books` vrátí seed data.
3. `GET /books/{id}` vrátí `404 NotFound` pro neexistující záznam.
4. `PATCH /books/{id}` správně změní `IsArchived`.
5. `DELETE /books/{id}` odstraní knihu z databáze.

### Další požadavky

1. Použijte `record` DTO.
2. Testy realizujte pomocí `Aspire.Hosting.Testing` a `xUnit`.
3. V testech kontrolujte jak HTTP odpověď, tak skutečný stav databáze.
4. Připravte `.http` soubor pro ruční vyzkoušení endpointů.
