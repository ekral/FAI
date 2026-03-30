using UTB.School.Contracts;

namespace UTB.School.Web
{
    public class SchoolService(HttpClient httpClient)
    {
        public async Task<StudentDto[]?> GetStudentsAsync()
        {
            StudentDto[]? students = await httpClient.GetFromJsonAsync<StudentDto[]>("/students");

            return students;
        }
    }
}
