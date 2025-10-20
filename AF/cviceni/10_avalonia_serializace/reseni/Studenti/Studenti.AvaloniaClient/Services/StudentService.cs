using Studenti.AvaloniaClient.ViewModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Studenti.AvaloniaClient.Services
{
    public class StudentService(HttpClient http) : IStudentService
    {
        private readonly HttpClient http = http;

        public Task<Student[]?> GetAllStudentsAsync()
        {
            Task<Student[]?> students = http.GetFromJsonAsync<Student[]>("/students");

            return students;
        }

        public Task UpdateStudentAsync(Student student)
        {
            return http.PutAsJsonAsync($"/students/{student.StudentId}", student);
        }
    }
}
