using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using UTB.PublicLibrary.Db;
using YamlDotNet.Core.Tokens;

namespace UTB.PublicLibrary.Tests.Tests
{
    public class TestFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;
        HttpClient? keycloakClient;
        private string? idToken;
        private string? connectionString;

        public HttpClient HttpClient { get; private set; } = null!;

        public async ValueTask InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_PublicLibrary_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            app = await builder.BuildAsync(TestContext.Current.CancellationToken);

            await app.StartAsync(TestContext.Current.CancellationToken);

            await app.ResourceNotifications.WaitForResourceHealthyAsync("keycloak", TestContext.Current.CancellationToken);

            keycloakClient = app.CreateHttpClient("keycloak", "https");

            var response = await keycloakClient.PostAsync("/realms/utb-publiclibrary/protocol/openid-connect/token", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", "utb-publiclibrary-tests" },
                { "username", "karel" },
                { "password", "karel" },
                { "scope", "openid" } // Důležité pro získání OIDC tokenu
            }));

            response.EnsureSuccessStatusCode();

            TokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>() ?? throw new Xunit.Sdk.XunitException("Token endpoint returned null TokenResponse.");

            if (string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                throw new Xunit.Sdk.XunitException("TokenResponse does not contain AccessToken.");
            }

            idToken = tokenResponse.IdToken;

            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);

            HttpClient = app.CreateHttpClient("webapi", "https");
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);

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
            if (keycloakClient is not null )
            {
                if (idToken is not null)
                {
                    var response = await keycloakClient.PostAsync("/realms/utb-publiclibrary/protocol/openid-connect/logout",
                        new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "id_token_hint", idToken }
                        }));
                }

                keycloakClient.Dispose();
            }

            HttpClient?.Dispose();

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