using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using UTB.Library.Db;

namespace UTB.Library.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;
        private HttpClient client = null!;
        private string? connectionString;

        public HttpClient HttpClient => client;

        public async ValueTask InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_Library_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            app = await builder.BuildAsync();

            await app.StartAsync(TestContext.Current.CancellationToken);

            client = app.CreateHttpClient("webapi");

            connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);

            using (LibraryContext context = CreateContext())
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            }

            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            client.Dispose();
            await app.DisposeAsync();

            GC.SuppressFinalize(this);
        }

        public LibraryContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                    .UseNpgsql(connectionString)
                    .Options;

            return new LibraryContext(options);
        }
    }
}
