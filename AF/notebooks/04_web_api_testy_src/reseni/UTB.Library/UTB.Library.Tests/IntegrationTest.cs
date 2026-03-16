using Aspire.Hosting;
using k8s.KubeConfigModels;
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
        public async Task Test()
        {
            using var context = fixture.CreateContext();

            var authorDto = new AuthorDto(0, "Franz Kafka");

            var response = await fixture.HttpClient.PostAsJsonAsync("/authors", authorDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            AuthorDto? responseAuthorDto = await response.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(responseAuthorDto);
            Assert.Equal(authorDto.Name, responseAuthorDto.Name);
            Assert.NotNull(response.Headers.Location);
            Assert.Equal($"/authors/{responseAuthorDto.Id}", response.Headers.Location.ToString());

            var author = context.Authors.Find(responseAuthorDto.Id);

            Assert.NotNull(author);
            Assert.Equal(authorDto.Name, author.Name);
       }
    }
}
