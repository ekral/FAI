using System.Net.Http.Json;
using UTB.PublicLibrary.Contracts;

namespace UTB.PublicLibrary.Tests.Tests
{
    [Collection("Database collection")]
    public class BookTests(TestFixture fixture)
    {
        private readonly TestFixture fixture = fixture;

        [Fact(Skip = "TODO: Implement as a student exercise.")]
        public Task CreateBook_ReturnsCreatedAndPersistsBook()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetBooks_ReturnsAllBooks()
        {
            var response = await fixture.HttpClient.GetAsync("/books", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            BookDto[]? books = await response.Content.ReadFromJsonAsync<BookDto[]>(TestContext.Current.CancellationToken);

            Assert.NotNull(books);
            Assert.True(books.Length > 2);
            Assert.Contains(books, book => book.Title == "Kytice" && book.IsArchived == false);
            Assert.Contains(books, book => book.Title == "Bila Nemoc" && book.IsArchived == false);
            Assert.Contains(books, book => book.Title == "Babicka" && book.IsArchived == true);
        }

        [Fact(Skip = "TODO: Implement as a student exercise.")]
        public Task GetBookById_ReturnsOkAndBook_WhenBookExists()
        {
            return Task.CompletedTask;
        }

        [Fact(Skip = "TODO: Implement as a student exercise.")]
        public Task GetBookById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            return Task.CompletedTask;
        }

        [Fact(Skip = "TODO: Implement as a student exercise.")]
        public Task DeleteBook_DeletesAndReturnsNoContent_WhenBookExists()
        {
            return Task.CompletedTask;
        }

        [Fact(Skip = "TODO: Implement as a student exercise.")]
        public Task UpdateBook_ReturnsNoContentAndUpdatesBook()
        {
            return Task.CompletedTask;
        }

        [Fact(Skip = "TODO: Implement as a student exercise.")]
        public Task PatchBookArchiveState_ReturnsNoContentAndUpdatesFlag()
        {
            return Task.CompletedTask;
        }
    }
}