using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using UTB.School.Db;

namespace UTB.School.Tests.Tests
{
    public class TestFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;
        private string? connectionString;
        public HttpClient HttpClient { get; private set; } = null!;

        public async ValueTask InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_School_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            app = await builder.BuildAsync(TestContext.Current.CancellationToken);

            await app.StartAsync(TestContext.Current.CancellationToken);

            await app.ResourceNotifications.WaitForResourceHealthyAsync("database", TestContext.Current.CancellationToken);
            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);

            connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);
            HttpClient = app.CreateHttpClient("webapi", "https");

            using var context = CreateContext();

            await context.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
            await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

            Student jan = new() { Name = "Jan", IsActive = true };
            Student eva = new() { Name = "Eva", IsActive = true };
            Student petr = new() { Name = "Petr", IsActive = false };

            context.Students.AddRange(jan, eva, petr);

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            HttpClient.Dispose();
            await app.DisposeAsync();

            GC.SuppressFinalize(this);
        }

        public SchoolContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                    .UseNpgsql(connectionString)
                    .Options;

            var context = new SchoolContext(options);

            return context;
        }
    }
}
