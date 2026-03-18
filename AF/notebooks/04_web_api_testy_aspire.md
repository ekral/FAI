# 04 Testování Minimal Web API s Aspire

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu si ukážeme moderní přístup k integračním testům pomocí [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) a frameworku [xUnit](https://xunit.net/). Aspire umožňuje testovat celou aplikaci včetně závislostí, jako je databáze, v kontejnerizovaném prostředí.

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

---

DTO pro přenos dat:

`AuthorRequestDto` se používá ve vstupu (`POST /authors`) a neobsahuje `Id`, protože to přidělí databáze. `AuthorDto` se používá ve výstupu API a `Id` obsahuje.

```csharp
namespace UTB.Library.Contracts
{
    public record AuthorRequestDto(string Name);
    public record AuthorDto(int Id, string Name);
}
```

A dále si nadefinujeme následující WebAPI metody:

---

```csharp
static async Task<Created<AuthorDto>> CreateAuthor(AuthorRequestDto authorRequestDto, LibraryContext context)
{
    Author author = new() { Name = authorRequestDto.Name };

    context.Authors.Add(author);

    await context.SaveChangesAsync();

    AuthorDto resultDto = new(author.Id, author.Name);

    return TypedResults.Created($"/authors/{resultDto.Id}", resultDto);
}

static async Task<Ok<AuthorDto[]>> GetAuthors(LibraryContext context)
{
    AuthorDto[] authors = await context.Authors.Select(a => new AuthorDto(a.Id, a.Name)).ToArrayAsync();

    return TypedResults.Ok(authors);
}

static async Task<Results<NotFound, Ok<AuthorDto>>> GetAuthorById(int id, LibraryContext context)
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

static async Task<Results<NoContent, NotFound>> DeleteAuthor(int id, LibraryContext context)
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

Mapování metod na endpointy v `Program.cs`:

```csharp
app.MapPost("/authors", CreateAuthor);
app.MapGet("/authors", GetAuthors);
app.MapGet("/authors/{id:int}", GetAuthorById);
app.MapDelete("/authors/{id:int}", DeleteAuthor);
```

---

## Integrační testy s Aspire

Moderní přístup k testování Web API využívá [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview), který umožňuje spouštět celou aplikaci včetně závislostí (databáze, cache, messaging) v kontejnerizovaném prostředí během testů. To poskytuje skutečně integrační testy, které testují aplikaci jako celek.

### Nastavení Aspire projektu

Nejprve vytvoříme Aspire AppHost projekt, který definuje infrastrukturu aplikace. V testovacím prostředí používáme jednodušší konfiguraci PostgreSQL bez persistentních dat a bez dbmanagera pro ruční reset databáze.

```csharp
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;
IResourceBuilder<PostgresDatabaseResource> database;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("postgres-testing");

    database = postgres.AddDatabase("database");
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

    database = postgres.AddDatabase("database");

    builder.AddProject<Projects.UTB_Library_DbManager>("dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);
}

builder.AddProject<Projects.UTB_Library_WebApi>("webapi")
       .WithReference(database)
       .WaitFor(database);

builder.Build().Run();

```

---

### Integrační testy

Pro integrační testy použijeme `DistributedApplicationTestingBuilder`, který spustí celou aplikaci včetně databáze. Testy pak komunikují s API přes HTTP klient.

`TestFixture` připraví integrační prostředí pro všechny testy: spustí Aspire AppHost v režimu `Testing`, počká na dostupnost databáze a Web API, vytvoří `HttpClient` a poskytne metodu `CreateContext()` pro vytváření contextu pro práci s databázi (ověřujeme, že v databázi jsou po volání endpointu správná data). Také seeduje data pro čtení.

`DatabaseCollection` zajistí sdílení jedné instance `TestFixture` mezi testy ve stejné kolekci, takže testy používají stejnou testovací infrastrukturu a není potřeba ji opakovaně inicializovat.

Ukázkové integrační testy níže pokrývají klíčové scénáře: vytvoření záznamu a kontrolu perzistence dat, načtení seed dat po resetu databáze, negativní scénář `404 NotFound` a mazání záznamu s následným ověřením změny stavu.

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

            await context.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
            await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

            var capek = new Author { Name = "Karel Capek" };
            var nemcova = new Author { Name = "Bozena Nemcova" };
            var mnacko = new Author { Name = "Ladislav Mnacko" };

            context.Authors.AddRange(capek, nemcova, mnacko);

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
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

    [CollectionDefinition("Database collection", DisableParallelization = true)]
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
            var authorRequestDto = new AuthorRequestDto("Franz Kafka");

            var response = await fixture.HttpClient.PostAsJsonAsync("/authors", authorRequestDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            AuthorDto? responseAuthorDto = await response.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(responseAuthorDto);
            Assert.Equal(authorRequestDto.Name, responseAuthorDto.Name);
            Assert.NotNull(response.Headers.Location);
            Assert.EndsWith($"/authors/{responseAuthorDto.Id}", response.Headers.Location.ToString());

            using var context = fixture.CreateContext();

            Author? author = await context.Authors.FindAsync(responseAuthorDto.Id, TestContext.Current.CancellationToken);

            Assert.NotNull(author);
            Assert.Equal(authorRequestDto.Name, author.Name);
        }

        [Fact]
        public async Task GetAuthors_ReturnsAllAuthors()
        {
            var response = await fixture.HttpClient.GetAsync("/authors", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthorDto[]? authors = await response.Content.ReadFromJsonAsync<AuthorDto[]>(TestContext.Current.CancellationToken);

            Assert.NotNull(authors);
            Assert.True(authors.Length > 2);
            Assert.Contains(authors, a => a.Name == "Karel Capek");
            Assert.Contains(authors, a => a.Name == "Bozena Nemcova");
            Assert.Contains(authors, a => a.Name == "Ladislav Mnacko");
        }

        [Fact]
        public async Task GetAuthorById_ReturnsOkAndAuthor_WhenAuthorExists()
        {
            var response = await fixture.HttpClient.GetAsync("/authors/1", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthorDto? author = await response.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(author);
            Assert.Equal("Karel Capek", author.Name);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsNotFound_WhenDoesNotExist()
        {
            var response = await fixture.HttpClient.GetAsync("/authors/999", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAuthor_DeletesAndReturnsNoContent_WhenExists()
        {
            var macha = new Author { Name = "Karel Hynek Macha" };

            using (var context = fixture.CreateContext())
            {
                context.Authors.Add(macha);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);

                var response = await fixture.HttpClient.DeleteAsync($"/authors/{macha.Id}", TestContext.Current.CancellationToken);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            using (var context = fixture.CreateContext())
            {
                var authorDto = await context.Authors.FindAsync(macha.Id, TestContext.Current.CancellationToken);

                Assert.Null(authorDto);
            }
        }
    }
}
```

### Co dělají jednotlivé testy

`CreateAuthor_ReturnsCreatedAndPersistsAuthor` ověřuje, že `POST /authors` vrátí `201 Created`, vrátí DTO nového autora, nastaví hlavičku `Location` a že je autor skutečně uložen v databázi.

`GetAuthors_ReturnsAllAuthors` nejdřív resetuje databázi přes `POST /reset-db`, potom volá `GET /authors` a ověřuje návrat seed dat v očekávaném počtu.

`GetAuthorById_ReturnsOkAndAuthor_WhenAuthorExists` ověřuje scénář, kdy autor existuje a vrátí autora.

`GetAuthorById_ReturnsNotFound_WhenDoesNotExist` ověřuje negativní scénář, kdy API pro neexistující záznam vrací správně `404 NotFound`.

`DeleteAuthor_DeletesAndReturnsNoContent_WhenExists` ověřuje mazání přes `DELETE /authors/1`, návrat `204 NoContent` a následně potvrzuje odstranění dalším `GET` dotazem.

### Shrnutí

Integrační testy s Aspire testují aplikaci přes HTTP jako celek, včetně reálné databáze běžící v kontejneru. Tento přístup dává jednoznačný a praktický obraz toho, jak ověřovat chování API v podmínkách blízkých produkčnímu prostředí.