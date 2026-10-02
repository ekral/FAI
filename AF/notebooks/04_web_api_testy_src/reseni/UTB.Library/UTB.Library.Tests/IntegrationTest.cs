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
