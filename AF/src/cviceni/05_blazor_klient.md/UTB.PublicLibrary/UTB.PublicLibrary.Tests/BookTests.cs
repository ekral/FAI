using System.Net.Http.Json;
using UTB.PublicLibrary.Contracts;
using UTB.PublicLibrary.Db;

namespace UTB.PublicLibrary.Tests.Tests
{
    [Collection("Database collection")]
    public class BookTests(TestFixture fixture)
    {
        private readonly TestFixture fixture = fixture;

        [Fact]
        public async Task CreateBook_ReturnsCreatedAndPersistsBook()
        {
            var bookRequestDto = new BookRequestDto("R.U.R.", true);

            var response = await fixture.HttpClient.PostAsJsonAsync("/books", bookRequestDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            BookDto? bookDto = await response.Content.ReadFromJsonAsync<BookDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(bookDto);
            Assert.Equal(bookRequestDto.Title, bookDto.Title);
            Assert.NotNull(response.Headers.Location);
            Assert.EndsWith($"/books/{bookDto.Id}", response.Headers.Location.ToString());

            using var context = fixture.CreateContext();

            Book? book = await context.Books.FindAsync([bookDto.Id], TestContext.Current.CancellationToken);

            Assert.NotNull(book);
            Assert.Equal(bookRequestDto.Title, book.Title);
            Assert.Equal(bookRequestDto.IsArchived, book.IsArchived);
        }

        [Fact]
        public async Task GetBooks_ReturnsAllBooks()
        {
            var response = await fixture.HttpClient.GetAsync("/books", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            BookDto[]? bookDtos = await response.Content.ReadFromJsonAsync<BookDto[]>(TestContext.Current.CancellationToken);

            Assert.NotNull(bookDtos);
            Assert.True(bookDtos.Length > 2);
            Assert.Contains(bookDtos, book => book.Title == "Kytice" && book.IsArchived == false);
            Assert.Contains(bookDtos, book => book.Title == "Bila Nemoc" && book.IsArchived == false);
            Assert.Contains(bookDtos, book => book.Title == "Babicka" && book.IsArchived == true);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkAndBook_WhenBookExists()
        {
            var response = await fixture.HttpClient.GetAsync("/books/1", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            BookDto? bookDto = await response.Content.ReadFromJsonAsync<BookDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(bookDto);
            Assert.Equal("Kytice", bookDto.Title);
            Assert.False(bookDto.IsArchived);
        }

        [Fact]
        public async Task GetBookById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var response = await fixture.HttpClient.GetAsync("/books/999", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_DeletesAndReturnsNoContent_WhenBookExists()
        {
            var maj = new Book { Title = "Maj", IsArchived = false };

            using (var context = fixture.CreateContext())
            {
                context.Books.Add(maj);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            var response = await fixture.HttpClient.DeleteAsync($"/books/{maj.Id}", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            using (var context = fixture.CreateContext())
            {
                var book = await context.Books.FindAsync([maj.Id], TestContext.Current.CancellationToken);

                Assert.Null(book);
            }
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContentAndUpdatesBook()
        {
            var maj = new Book { Title = "Maj", IsArchived = false };

            using (var context = fixture.CreateContext())
            {
                context.Books.Add(maj);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            BookRequestDto bookRequestDto = new("Vojna a mir", true);

            var response = await fixture.HttpClient.PutAsJsonAsync($"/books/{maj.Id}", bookRequestDto,TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            using (var context = fixture.CreateContext())
            {
                var book = await context.Books.FindAsync([maj.Id], TestContext.Current.CancellationToken);

                Assert.NotNull(book);
                Assert.Equal(bookRequestDto.Title, book.Title);
                Assert.Equal(bookRequestDto.IsArchived, book.IsArchived);
            }
        }

        [Fact]
        public async Task PatchBookArchiveState_ReturnsNoContentAndUpdatesFlag()
        {
            var maj = new Book { Title = "Maj", IsArchived = false };

            using (var context = fixture.CreateContext())
            {
                context.Books.Add(maj);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            BookPatchRequestDto bookPatchRequestDto = new(true);

            var response = await fixture.HttpClient.PatchAsJsonAsync($"/books/{maj.Id}", bookPatchRequestDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            using (var context = fixture.CreateContext())
            {
                var book = await context.Books.FindAsync([maj.Id], TestContext.Current.CancellationToken);

                Assert.NotNull(book);
                Assert.Equal(bookPatchRequestDto.IsArchived, book.IsArchived);
            }
        }
    }
}