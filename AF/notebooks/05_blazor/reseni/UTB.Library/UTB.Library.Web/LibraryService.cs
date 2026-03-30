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

        public async Task<AuthorDto?> GetAuthorByIdAsync(int authorId)
        {
            AuthorDto? author = await httpClient.GetFromJsonAsync<AuthorDto>($"/authors/{authorId}");

            return author;
        }

        public async Task CreateAuthorAsync(AuthorRequestDto authorRequestDto)
        {
            await httpClient.PostAsJsonAsync("/authors", authorRequestDto);
        }

        public async Task UpdateAuthorAsync(int authorId, AuthorRequestDto authorRequestDto)
        {
            await httpClient.PutAsJsonAsync($"/authors/{authorId}", authorRequestDto);
        }

        public async Task DeleteAuthorAsync(int authorId)
        {
            await httpClient.DeleteAsync($"/authors/{authorId}");
        }

    }
}
