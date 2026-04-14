using UTB.PublicLibrary.Contracts;

namespace UTB.PublicLibrary.Web
{
    public class LibraryService(HttpClient httpClient)
    {
        public async Task<BookDto[]?> GetBooksAsync()
        {
            BookDto[]? books = await httpClient.GetFromJsonAsync<BookDto[]>("/books");

            return books;
        }

        public async Task<BookDto?> GetBookAsync(int id)
        {
            BookDto? book = await httpClient.GetFromJsonAsync<BookDto>($"/books/{id}");

            return book;
        }

        public async Task CreateBookAsync(BookRequestDto book)
        {
            var response = await httpClient.PostAsJsonAsync("/books", book);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateBookAsync(int id, BookRequestDto book)
        {
            var response = await httpClient.PutAsJsonAsync($"/books/{id}", book);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBookAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"/books/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
