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
    }
}
