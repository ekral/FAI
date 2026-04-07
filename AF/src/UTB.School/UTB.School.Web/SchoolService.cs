using System.Net;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using UTB.School.Contracts;
using static System.Net.WebRequestMethods;

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

        public async Task<Result<StudentDto>> GetStudentAsync(int studentId)
        {
            try
            {
                var response = await httpClient.GetAsync($"/students/{studentId}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StudentDto? student = await response.Content.ReadFromJsonAsync<StudentDto>();

                    if (student is null)
                    {
                        return Result<StudentDto>.Failure("Nepodařilo se načíst data studenta.");
                    }

                    return Result<StudentDto>.Success(student);
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result<StudentDto>.Failure("Požadovaný záznam neexistuje.");
                }

                return Result<StudentDto>.Failure($"Načtení studenta selhalo. Neočekávaný stav: {(int)response.StatusCode} ({response.StatusCode})");
            }
            catch (HttpRequestException)
            {
                return Result<StudentDto>.Failure("API není dostupné");
            }
            catch (TaskCanceledException)
            {
                return Result<StudentDto>.Failure("Timeout");
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

        public async Task<Result> UpdateStudentAsync(int studentId, StudentRequestDto requestDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"/students/{studentId}", requestDto);

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Result.Success();
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Failure("Požadovaný záznam neexistuje.");
                }

                return Result.Failure($"Uložení změn selhalo. Neočekávaný stav: {(int)response.StatusCode} ({response.StatusCode})");
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

        public async IAsyncEnumerable<StudentDto> StreamSseMessagesAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            using var response = await httpClient.GetAsync("/stream", HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            
            SseParser<string> parser = SseParser.Create(stream);

            await foreach (var sseEvent in parser.EnumerateAsync(ct))
            {
                if (!string.IsNullOrEmpty(sseEvent.Data))
                {
                    // SSE vrací JSON string, parsuj ho na StudentDto
                    var student = System.Text.Json.JsonSerializer.Deserialize<StudentDto>(sseEvent.Data);
                    if (student is not null)
                    {
                        yield return student;
                    }
                }
            }
        }
    }
}
