# 04 Testování Minimal Web API s Aspire

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu si probereme [testování Web API s produkční databází](https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database) a frameworkem [xUnit](https://xunit.net/). Dále si ukážeme moderní přístup k integračním testům pomocí [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview), který umožňuje testovat celou aplikaci včetně závislostí jako databáze v kontejnerizovaném prostředí.

---

Nejprve si nadefinujeme entity a DbContext:

```csharp
namespace UTB.Library.Db
{
    public class Author
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
    {
        public DbSet<Author> Authors { get; set; }
    }
}
```

A dále si nadefinujeme následující WebAPI metody:

---

```csharp
public static async Task<Created<AuthorDto>> CreateAuthor(AuthorDto authorDto, LibraryContext context)
{
    Author author = new() { Name = authorDto.Name };

    context.Authors.Add(author);

    await context.SaveChangesAsync();

    AuthorDto resultDto = new(author.Id, author.Name);

    return TypedResults.Created($"/authors/{resultDto.Id}", resultDto);
}

public static async Task<Ok<AuthorDto[]>> GetAuthors(LibraryContext context)
{
    AuthorDto[] authors = await context.Authors.Select(a => new AuthorDto(a.Id, a.Name)).ToArrayAsync();

    return TypedResults.Ok(authors);
}

public static async Task<Results<NotFound, Ok<AuthorDto>>> GetAuthorById(int id, LibraryContext context)
{
    if (await context.Authors.FindAsync(id) is Author author)
    {
        AuthorDto authorDto = new(author.Id, author.Name);

        return TypedResults.Ok(authorDto);
    }
    else
    {
        return TypedResults.NotFound();
    }
}

public static async Task<Results<NoContent, NotFound>> UpdateAuthor(int id, AuthorDto authorDto, LibraryContext context)
{
    if (await context.Authors.FindAsync(id) is Author author)
    {
        author.Name = authorDto.Name;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}

public static async Task<Results<NoContent, NotFound>> DeleteAuthor(int id, LibraryContext context)
{
    if (await context.Authors.FindAsync(id) is Author author)
    {
        context.Authors.Remove(author);

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}

```

---

DTO pro přenos dat:

```csharp
namespace UTB.Library.Contracts
{
    public record AuthorDto(int Id, string Name);
}
```

---

## Testování s produkční databází (Unit testy)

Nástroj xUnit organizuje testovací metody do tříd, kdy testovací metody ve třídě se spouští sekvenčně a testy v různých třídách se potom pouští souběžně. Pro testování s produkční databází používáme Class Fixture pro sdílení databáze mezi testy.

```csharp
public class DatabaseFixture
{
    private static readonly Lock _lock = new();
    private static bool _databaseInitialized = false;

    public DatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using LibraryContext context = CreateContext();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Authors.AddRange(
                    new Author() { Id = 1, Name = "Karel Čapek" },
                    new Author() { Id = 2, Name = "Jaroslav Hašek" },
                    new Author() { Id = 3, Name = "Bohumil Hrabal" }
                    );

                context.SaveChanges();

                _databaseInitialized = true;
            }
        }
    }

    public LibraryContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite("DataSource=test.db")
            .Options;

        return new LibraryContext(options);
    }
}
```

Vlastní testy pak vypadají následovně:

```csharp
public class UnitTestWebAPI(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private DatabaseFixture Fixture { get; } = fixture;

    [Fact]
    public async Task GetAuthors_ShouldReturnAllAuthors()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();

        // Act
        var result = await WebApiVersion1.GetAuthors(context);

        // Assert
        Assert.Equal(3, result.Value?.Length);
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturnAuthor_WhenAuthorExists()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();

        // Act
        var result = await WebApiVersion1.GetAuthorById(1, context);

        // Assert
        Ok<AuthorDto> okAuthor = Assert.IsType<Ok<AuthorDto>>(result.Result);

        Assert.Equal(1, okAuthor.Value?.Id);
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturnNotFound_WhenAuthorDoesNotExist()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();

        // Act
        var result = await WebApiVersion1.GetAuthorById(999, context);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task CreateAuthor_ShouldAddAuthor()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();
        
        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        var newAuthor = new AuthorDto(0, "New Author");

        // Act
        _ = await WebApiVersion1.CreateAuthor(newAuthor, context);

        context.ChangeTracker.Clear();

        // Assert
        Assert.Equal(4, await context.Authors.CountAsync());
        Author? author = await context.Authors.FindAsync(4);
        Assert.NotNull(author);
        Assert.Equal("New Author", author.Name);
    }

    [Fact]
    public async Task UpdateAuthor_ShouldUpdateAuthor_WhenAuthorExists()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        var updatedAuthor = new AuthorDto(0, "Updated Author");

        // Act
        var result = await WebApiVersion1.UpdateAuthor(1, updatedAuthor, context);

        context.ChangeTracker.Clear();

        // Assert
        Assert.IsType<NoContent>(result.Result);
        Author? author = await context.Authors.FindAsync(1);
        Assert.NotNull(author);
        Assert.Equal("Updated Author", author.Name);
    }

    [Fact]
    public async Task UpdateAuthor_ShouldReturnNotFound_WhenAuthorDoesNotExist()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        var updatedAuthor = new AuthorDto(0, "Updated Author");

        // Act
        var result = await WebApiVersion1.UpdateAuthor(999, updatedAuthor, context);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task DeleteAuthor_ShouldRemoveAuthor_WhenAuthorExists()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        // Act
        var result = await WebApiVersion1.DeleteAuthor(1, context);

        context.ChangeTracker.Clear();

        // Assert
        Assert.IsType<NoContent>(result.Result);
        Assert.Null(await context.Authors.FindAsync(1));
    }

    [Fact]
    public async Task DeleteAuthor_ShouldReturnNotFound_WhenAuthorDoesNotExist()
    {
        // Arrange
        using LibraryContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        // Act
        var result = await WebApiVersion1.DeleteAuthor(999, context);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }
}
```

## Integrační testy s Aspire

Moderní přístup k testování Web API využívá [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview), který umožňuje spouštět celou aplikaci včetně závislostí (databáze, cache, messaging) v kontejnerizovaném prostředí během testů. To poskytuje skutečně integrační testy, které testují aplikaci jako celek.

### Nastavení Aspire projektu

Nejprve vytvoříme Aspire AppHost projekt, který definuje infrastrukturu aplikace:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("postgres-testing");
}
else
{
    postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin(c =>
                      {
                          c.WithLifetime(ContainerLifetime.Persistent);
                          c.WithImage("dpage/pgadmin4:latest");
                      })
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent);
}

var database = postgres.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

builder.AddProject<Projects.UTB_Library_WebApi>("webapi")
       .WithReference(database)
       .WaitFor(database);

builder.Build().Run();
```

V testovacím prostředí používáme jednodušší konfiguraci PostgreSQL bez persistentních dat.

### Integrační testy

Pro integrační testy použijeme `DistributedApplicationTestingBuilder`, který spustí celou aplikaci včetně databáze. Testy pak komunikují s API přes HTTP klient.

```csharp
using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using UTB.Library.Contracts;
using UTB.Library.Db;

namespace UTB.Library.Tests
{
    public class TestFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;
        private string? connectionString;
        public HttpClient HttpClient { get; private set; } = null!;

        public async ValueTask InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_Library_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            app = await builder.BuildAsync(TestContext.Current.CancellationToken);

            await app.StartAsync(TestContext.Current.CancellationToken);

            await app.ResourceNotifications.WaitForResourceHealthyAsync("database", TestContext.Current.CancellationToken);
            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);

            connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);
            HttpClient = app.CreateHttpClient("webapi", "https");

            using var context = CreateContext();

            await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            HttpClient.Dispose();
            await app.DisposeAsync();

            GC.SuppressFinalize(this);
        }

        public LibraryContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                    .UseNpgsql(connectionString)
                    .Options;

            var context = new LibraryContext(options);

            return context;
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<TestFixture>
    {
    }

    [Collection("Database collection")]
    public class IntegrationTest(TestFixture fixture)
    {
        private readonly TestFixture fixture = fixture;

        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAndPersistsAuthor()
        {   
            var authorDto = new AuthorDto(0, "Franz Kafka");

            var response = await fixture.HttpClient.PostAsJsonAsync("/authors", authorDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            AuthorDto? responseAuthorDto = await response.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(responseAuthorDto);
            Assert.Equal(authorDto.Name, responseAuthorDto.Name);
            Assert.NotNull(response.Headers.Location);
            Assert.EndsWith($"/authors/{responseAuthorDto.Id}", response.Headers.Location.ToString());

            using var context = fixture.CreateContext();

            Author? author = await context.Authors.FindAsync(responseAuthorDto.Id, TestContext.Current.CancellationToken);

            Assert.NotNull(author);
            Assert.Equal(authorDto.Name, author.Name);
        }

        [Fact]
        public async Task GetAuthors_ReturnsAllAuthors()
        {
            // Reset database to known state
            var resetResponse = await fixture.HttpClient.PostAsync("/reset-db", null, TestContext.Current.CancellationToken);
            resetResponse.EnsureSuccessStatusCode();

            var response = await fixture.HttpClient.GetAsync("/authors", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthorDto[]? authors = await response.Content.ReadFromJsonAsync<AuthorDto[]>(TestContext.Current.CancellationToken);

            Assert.NotNull(authors);
            Assert.Equal(3, authors.Length);
            Assert.Contains(authors, a => a.Name == "Karel Capek");
        }

        [Fact]
        public async Task GetAuthorById_ReturnsAuthor_WhenExists()
        {
            var response = await fixture.HttpClient.GetAsync("/authors/1", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthorDto? author = await response.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(author);
            Assert.Equal(1, author.Id);
            Assert.Equal("Karel Capek", author.Name);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsNotFound_WhenDoesNotExist()
        {
            var response = await fixture.HttpClient.GetAsync("/authors/999", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuthor_UpdatesAndReturnsNoContent_WhenExists()
        {
            var updateDto = new AuthorDto(0, "Updated Name");

            var response = await fixture.HttpClient.PutAsJsonAsync("/authors/1", updateDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify the update
            var getResponse = await fixture.HttpClient.GetAsync("/authors/1", TestContext.Current.CancellationToken);
            AuthorDto? updatedAuthor = await getResponse.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(updatedAuthor);
            Assert.Equal("Updated Name", updatedAuthor.Name);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsNotFound_WhenDoesNotExist()
        {
            var updateDto = new AuthorDto(0, "Updated Name");

            var response = await fixture.HttpClient.PutAsJsonAsync("/authors/999", updateDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAuthor_DeletesAndReturnsNoContent_WhenExists()
        {
            var response = await fixture.HttpClient.DeleteAsync("/authors/1", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify deletion
            var getResponse = await fixture.HttpClient.GetAsync("/authors/1", TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsNotFound_WhenDoesNotExist()
        {
            var response = await fixture.HttpClient.DeleteAsync("/authors/999", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
```

### Klíčové rozdíly mezi unit a integračními testy

- **Unit testy**: Testují jednotlivé metody s přímým přístupem k DbContext, používají transakce pro izolaci
- **Integrační testy**: Testují celou aplikaci přes HTTP API, spouští skutečnou databázi v kontejneru, ověřují end-to-end funkcionalitu

Integrační testy s Aspire poskytují vyšší jistotu, že aplikace funguje správně v reálném prostředí, ale jsou pomalejší a vyžadují více zdrojů.