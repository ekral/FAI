using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using UTB.Library.Contracts;
using UTB.Library.Db;

namespace UTB.Library.Tests.Tests
{


    public class IntegrationTest1
    {
        [Fact]
        public async Task Test()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_Library_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            var app = await builder.BuildAsync(TestContext.Current.CancellationToken);

            await app.StartAsync(TestContext.Current.CancellationToken);

            await app.ResourceNotifications.WaitForResourceHealthyAsync("database", TestContext.Current.CancellationToken);
            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);

            var connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);
            using var client = app.CreateHttpClient("webapi", "https");

            var options = new DbContextOptionsBuilder<LibraryContext>()
                    .UseNpgsql(connectionString)
                    .Options;

            using var context = new LibraryContext(options);

            await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

            AuthorDto authorDto = new AuthorDto(0, "Franz Kafka");

            var response = await client.PostAsJsonAsync("/authors", authorDto, TestContext.Current.CancellationToken);

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
