using UTB.Library.Contracts;

namespace UTB.Library.Web
{
    public class LibraryService(HttpClient httpClient)
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task<AuthorDto[]?> GetAuthorsAsync()
        {
            AuthorDto[]? authors = await httpClient.GetFromJsonAsync<AuthorDto[]>("/authors");

            return authors;
        }

        public async Task CreateAuthorAsync()
        {
            AuthorRequestDto authorDto = new AuthorRequestDto("Jules Verne");

            await httpClient.PostAsJsonAsync("/authors", authorDto);
        }

    }
}
