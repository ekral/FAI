using System.Net.Http.Json;
using UTB.Library.Contracts;
using UTB.Library.Db;
using YamlDotNet.Core.Tokens;

namespace UTB.Library.Tests
{

    [Collection("Database collection")]
    public class AuthorsIntegrationTests(DatabaseFixture fixture)
    {
        private readonly DatabaseFixture fixture = fixture;

        [Fact]
        public async Task GetAuthors_ReturnsAllSeededAuthors()
        {
            // Arrange
            Author nemcova = new() { Name = "Bozena Nemcova" };
            Author hasek = new() { Name = "Jaroslav Hasek" };
            Author mnacko = new() { Name = "Ladislav Mnacko" };

            using (var context = fixture.CreateContext())
            {
                context.Authors.AddRange(nemcova, hasek, mnacko);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            // Act
            var client = fixture.HttpClient;
            var response = await client.GetAsync("authors", TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var authors = await response.Content.ReadFromJsonAsync<AuthorDto[]>(TestContext.Current.CancellationToken);

            Assert.NotNull(authors);
            Assert.Contains(authors, a => a.Id == nemcova.Id && a.Name == nemcova.Name);
            Assert.Contains(authors, a => a.Id == hasek.Id && a.Name == hasek.Name);
            Assert.Contains(authors, a => a.Id == mnacko.Id && a.Name == mnacko.Name);
            Assert.True(authors.Length >= 3);
        }

        [Fact]
        public async Task CreateAuthor_WhenValid_ReturnsCreatedAndPersistsToDatabase()
        {
            // Arrange
            var polacek = new AuthorDto(0, "Karel Polacek");

            // Act - Vytvoření
            var client = fixture.HttpClient;
            HttpResponseMessage createResponse = await client.PostAsJsonAsync("authors", polacek, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            // Assert returned Author
            AuthorDto? createdAuthor = await createResponse.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(createdAuthor);
            Assert.Equal(polacek.Name, createdAuthor.Name);
            Assert.NotNull(createResponse.Headers.Location);
            Assert.Contains($"authors/{createdAuthor.Id}", createResponse.Headers.Location.ToString());

            // Assert Author in Database
            using var context = fixture.CreateContext();

            Author? authorInDatabase = await context.Authors.FindAsync(createdAuthor.Id, TestContext.Current.CancellationToken);

            Assert.NotNull(authorInDatabase);
            Assert.Equal(polacek.Name, authorInDatabase.Name);
        }

        [Fact]
        public async Task GetAuthorById_WhenExists_ReturnsOkAndExpectedAuthor()
        {
            // Arrange
            Author hrabal = new() { Name = "Bohumil Hrabal" };

            using (var context = fixture.CreateContext())
            {
                context.Authors.Add(hrabal);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            // Act
            var client = fixture.HttpClient;
            HttpResponseMessage response = await client.GetAsync($"authors/{hrabal.Id}", TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthorDto? author = await response.Content.ReadFromJsonAsync<AuthorDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(author);
            Assert.Equal(hrabal.Id, author.Id);
            Assert.Equal(hrabal.Name, author.Name);
        }

        [Fact]
        public async Task GetAuthorById_WhenNotExists_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 999999; // Hodnota, která v DB pravděpodobně nebude

            // Act
            var client = fixture.HttpClient;
            var response = await client.GetAsync($"authors/{nonExistentId}", TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuthor_WhenExists_UpdatesAuthorInDatabase()
        {
            // Arrange
            Author macha = new() { Name = "Karel Macha" };

            using (var context = fixture.CreateContext())
            {
                context.Authors.Add(macha);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            // Act: Upravím tohoto nového autora
            AuthorDto updatedAuthor = new(macha.Id, "Karel Hynek Macha");

            var client = fixture.HttpClient;
            var updateResponse = await client.PutAsJsonAsync($"authors/{updatedAuthor.Id}", updatedAuthor, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            // Assert Author in Database
            using (var context = fixture.CreateContext())
            {
                Author? authorInDatabase = await context.Authors.FindAsync(macha.Id, TestContext.Current.CancellationToken);

                Assert.NotNull(authorInDatabase);
                Assert.Equal(updatedAuthor.Name, authorInDatabase.Name);
            }
        }

        [Fact]
        public async Task UpdateAuthor_WhenNotExists_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 999999;
            var updatedAuthorDto = new AuthorDto(nonExistentId, "Nikdo");

            // Act
            var client = fixture.HttpClient;
            var response = await client.PutAsJsonAsync($"authors/{nonExistentId}", updatedAuthorDto, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAuthor_WhenExists_RemovesFromDatabase()
        {
            // Arrange
            Author vancura = new() { Name = "Vladislav Vancura" };

            using (var context = fixture.CreateContext())
            {
                context.Authors.Add(vancura);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            // Act
            var client = fixture.HttpClient;
            var deleteResponse = await client.DeleteAsync($"authors/{vancura.Id}", TestContext.Current.CancellationToken);

            // Assert
            // Standardní REST odpověď pro smazání je 204 NoContent (případně 200 OK)
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Kontrola přímo v databázi, že záznam už neexistuje
            using (var context = fixture.CreateContext())
            {
                Author? authorInDatabase = await context.Authors.FindAsync(vancura.Id, TestContext.Current.CancellationToken);

                Assert.Null(authorInDatabase);
            }
        }

        [Fact]
        public async Task DeleteAuthor_WhenNotExists_ReturnsNotFound()
        {
            // Arrange
            var client = fixture.HttpClient;

            int nonExistentAuthorId = 999999;

            // Act
            var response = await client.DeleteAsync($"authors/{nonExistentAuthorId}", TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
