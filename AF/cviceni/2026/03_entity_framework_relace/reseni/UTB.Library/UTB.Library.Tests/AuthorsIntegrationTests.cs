using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using UTB.Library.Db;

namespace UTB.Library.Tests
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

            string? connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);

            var options = new DbContextOptionsBuilder<LibraryContext>()
                    .UseNpgsql(connectionString)
                    .Options;

            using (LibraryContext context = new(options))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                Author a1 = new() { Name = "Karel Capek" };
                Author a2 = new() { Name = "Jaroslav Hasek" };
                Author a3 = new() { Name = "Bohumil Hrabal" };

                Book b1 = new() { Name = "R.U.R.", Authors = [a1] };
                Book b2 = new() { Name = "Svejk", Authors = [a2] };
                Book b3 = new() { Name = "Postriziny", Authors = [a3] };

                Member m1 = new() { Name = "Jiri" };
                Member m2 = new() { Name = "Alena" };
                Member m3 = new() { Name = "Karel" };

                Loan l1 = new() { Book = b1, Member = m1, Date = DateOnly.FromDateTime(DateTime.Now), LoanStatus = LoanStatus.Borrowed };
                Loan l2 = new() { Book = b2, Member = m2, Date = DateOnly.FromDateTime(DateTime.Now), LoanStatus = LoanStatus.Borrowed };
                Loan l3 = new() { Book = b3, Member = m3, Date = DateOnly.FromDateTime(DateTime.Now), LoanStatus = LoanStatus.Borrowed };

                context.Authors.AddRange(a1, a2, a3);
                context.Books.AddRange(b1, b2, b3);
                context.Members.AddRange(m1, m2, m3);
                context.Loans.AddRange(l1, l2, l3);

                await context.SaveChangesAsync();
            }

            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            client.Dispose();
            await app.DisposeAsync();

            GC.SuppressFinalize(this);
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }

    [Collection("Database collection")]
    public class AuthorsIntegrationTests(DatabaseFixture fixture)
    {
        private readonly DatabaseFixture fixture = fixture;

        [Fact]
        public async Task GetAuthors_ReturnsOkAndExpectedAuthors()
        {
            HttpClient client = fixture.HttpClient;

            var response = await client.GetAsync("authors", TestContext.Current.CancellationToken);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Author[]? authors = await response.Content.ReadFromJsonAsync<Author[]>(TestContext.Current.CancellationToken);
        
            Assert.NotNull(authors);

            Author[] expected = [ new() { Id = 1, Name = "Karel Capek" }, new() { Id = 2,  Name = "Jaroslav Hasek" } ];

            Assert.Equivalent(expected, authors);
        }
    }
}
