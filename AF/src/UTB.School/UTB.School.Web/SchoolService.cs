using OneOf;
using OneOf.Types;
using UTB.School.Contracts;

namespace UTB.School.Web
{
    public class SchoolService(HttpClient httpClient)
    {
        public async Task<OneOf<StudentDto[], string>> GetStudentsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("/students");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    StudentDto[]? students = await response.Content.ReadFromJsonAsync<StudentDto[]>();
                    return students ?? [];
                }

                return $"Načtení studentů selhalo. Neočekávaný stav: {(int)response.StatusCode} {response.ReasonPhrase}";
            }
            catch (HttpRequestException)
            {
                return "Nepodařilo se načíst studenty.";
            }
            catch (TaskCanceledException)
            {
                return "Vypršel časový limit požadavku.";
            }
        }

        public async Task<OneOf<Success, string>> CreateStudentAsync(StudentRequestDto requestDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/students", requestDto);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return new Success();
                }

                return $"Vytvoření studenta selhalo. Neočekávaný stav: {(int)response.StatusCode} {response.ReasonPhrase}";
            }
            catch (HttpRequestException)
            {
                return "Nepodařilo se připojit k serveru.";
            }
            catch (TaskCanceledException)
            {
                return "Vypršel časový limit požadavku.";
            }
        }

        public async Task<OneOf<Success, string>> DeleteStudentAsync(int studentId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/students/{studentId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new Success();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return "Požadovaný záznam neexistuje.";
                }

                return $"Smazání selhalo. Neočekávaný stav: {(int)response.StatusCode} {response.ReasonPhrase}";
            }
            catch (HttpRequestException)
            {
                return "Nepodařilo se připojit k serveru.";
            }
            catch (TaskCanceledException)
            {
                return "Vypršel časový limit požadavku.";
            }
        }
    }
}
