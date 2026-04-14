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

        public async Task DeleteBookAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"/books/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
