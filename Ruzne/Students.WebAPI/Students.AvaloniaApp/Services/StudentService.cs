using Students.AvaloniaApp.ViewModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Services
{
    public class StudentService(HttpClient http) : IStudentService
    {
        private readonly HttpClient http = http;

        public Task<StudentViewModel[]?> GetAllStudentsAsync()
        {
            Task<StudentViewModel[]?> students = http.GetFromJsonAsync<StudentViewModel[]>("/students");

            return students;
        }

        public Task UpdateStudentAsync(StudentViewModel student)
        {
            return http.PutAsJsonAsync($"/students/{student.StudentId}", student);
        }
    }
}
