using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using UTB.PublicLibrary.Db;

namespace UTB.PublicLibrary.Tests.Tests
{
    public class TestFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;
        private string? connectionString;
        public HttpClient HttpClient { get; private set; } = null!;

        public async ValueTask InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_PublicLibrary_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            app = await builder.BuildAsync(TestContext.Current.CancellationToken);

            await app.StartAsync(TestContext.Current.CancellationToken);

            await app.ResourceNotifications.WaitForResourceHealthyAsync("database", TestContext.Current.CancellationToken);
            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);

            connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);
            HttpClient = app.CreateHttpClient("webapi", "https");

            using var context = CreateContext();

            await context.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
            await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

            Book kytice = new() { Title = "Kytice", IsArchived = false };
            Book bilaNemoc = new() { Title = "Bila Nemoc", IsArchived = false };
            Book babicka = new() { Title = "Babicka", IsArchived = true };

            context.Books.AddRange(kytice, bilaNemoc, babicka);

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            HttpClient.Dispose();
            await app.DisposeAsync();

            GC.SuppressFinalize(this);
        }

        public PublicLibraryContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<PublicLibraryContext>()
                    .UseNpgsql(connectionString)
                    .Options;

            var context = new PublicLibraryContext(options);

            return context;
        }
    }
}