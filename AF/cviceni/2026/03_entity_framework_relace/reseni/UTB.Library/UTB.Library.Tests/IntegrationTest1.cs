using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace UTB.Library.Tests.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;
        private HttpClient client = null!;

        public HttpClient HttpClient => client;

        public async ValueTask InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_Library_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            app = await builder.BuildAsync();

            await app.StartAsync(TestContext.Current.CancellationToken);

            client = app.CreateHttpClient("webapi");

            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            client.Dispose();
            await app.DisposeAsync();
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }

    [Collection("Database collection")]
    public class IntegrationTest1(DatabaseFixture fixture)
    {
        private readonly DatabaseFixture fixture = fixture;

        [Fact]
        public async Task GetWebResourceRootReturnsOkStatusCode()
        {
            // Arrange

            HttpClient client = fixture.HttpClient;

            var response = await client.GetAsync("authors", TestContext.Current.CancellationToken);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
