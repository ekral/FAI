using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Students.BlazorApp.Pages
{
  
    public partial class EditStudent(HttpClient client, NavigationManager navigation)
    {
        private readonly HttpClient client = client;
        private readonly NavigationManager navigation = navigation;

        [Parameter]
        public int StudentId { get; set; }

        [SupplyParameterFromForm]
        public Student? Student { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Student = await client.GetFromJsonAsync<Student>($"/students/{StudentId}");
        }

        private async Task ValidSubmit()
        {
            HttpResponseMessage result = await client.PutAsJsonAsync($"/students/{Student?.StudentId}", Student);

            navigation.NavigateTo("/students");
        }
    }
}