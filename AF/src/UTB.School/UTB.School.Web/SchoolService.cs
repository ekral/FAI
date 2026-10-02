using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using UTB.School.Contracts;

namespace UTB.School.Web
{
    public class SchoolService(HttpClient httpClient)
    {
        public async Task<StudentDto[]?> GetStudentsAsync()
        {
            var students = await httpClient.GetFromJsonAsync<StudentDto[]>("/students");

            return students;
        }

        public async Task<StudentDto?> GetStudentAsync(int studentId)
        {
            StudentDto? student = await httpClient.GetFromJsonAsync<StudentDto>($"/students/{studentId}");

            return student;   
        }

        public async Task CreateStudentAsync(StudentRequestDto requestDto)
        {
            var response = await httpClient.PostAsJsonAsync("/students", requestDto);

            response.EnsureSuccessStatusCode();      
        }

        public async Task UpdateStudentAsync(int studentId, StudentRequestDto requestDto)
        {
            var response = await httpClient.PutAsJsonAsync($"/students/{studentId}", requestDto);

            response.EnsureSuccessStatusCode();
               
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var response = await httpClient.DeleteAsync($"/students/{studentId}");

            response.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<StudentDto> StreamSseMessagesAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            using var response = await httpClient.GetAsync("/stream", HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);

            SseParser<string> parser = SseParser.Create(stream);

            await foreach (SseItem<string> sseEvent in parser.EnumerateAsync(ct))
            {
                if(sseEvent.EventType == "init")
                {
                    continue;
                }

                StudentDto? student = JsonSerializer.Deserialize<StudentDto>(sseEvent.Data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (student is null)
                {
                    continue;
                }

                yield return student;
            }
        }
    }
}
