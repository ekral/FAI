using System.Net;
using UTB.School.Contracts;

namespace UTB.School.Web
{
    public class SchoolService(HttpClient httpClient)
    {
        public async Task<Result<StudentDto[]>> GetStudentsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("/students");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StudentDto[]? students = await response.Content.ReadFromJsonAsync<StudentDto[]>();
                    return Result<StudentDto[]>.Success(students ?? []);
                }

                return Result<StudentDto[]>.Failure($"Načtení studentů selhalo. Neočekávaný stav: {(int)response.StatusCode} ({response.StatusCode})");
            }
            catch (HttpRequestException)
            {
                return Result<StudentDto[]>.Failure("API není dostupné");
            }
            catch (TaskCanceledException)
            {
                return Result<StudentDto[]>.Failure("Timeout");
            }
        }

        public async Task<Result> CreateStudentAsync(StudentRequestDto requestDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/students", requestDto);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return Result.Success();
                }

                return Result.Failure($"Vytvoření studenta selhalo. Neočekávaný stav: {(int)response.StatusCode} ({response.StatusCode})");
            }
            catch (HttpRequestException)
            {
                return Result.Failure("API není dostupné");
            }
            catch (TaskCanceledException)
            {
                return Result.Failure("Timeout");
            }
        }

        public async Task<Result> DeleteStudentAsync(int studentId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/students/{studentId}");

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Result.Success();
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Failure("Požadovaný záznam neexistuje.");
                }

                return Result.Failure($"Smazání selhalo. Neočekávaný stav: {(int)response.StatusCode} ({response.StatusCode})");
            }
            catch (HttpRequestException)
            {
                return Result.Failure("API není dostupné");
            }
            catch (TaskCanceledException)
            {
                return Result.Failure("Timeout");
            }
        }
    }
}
