
using System.Net.Http.Json;

namespace Students.BlazorApp.Pages
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public required bool Studuje { get; set; }
    }

    public partial class Studenti(HttpClient client)
    {
        private readonly HttpClient client = client;
        private Student[]? studenti;

        protected override async Task OnInitializedAsync()
        {
            studenti = await client.GetFromJsonAsync<Student[]>("/students");
        }
    }
}