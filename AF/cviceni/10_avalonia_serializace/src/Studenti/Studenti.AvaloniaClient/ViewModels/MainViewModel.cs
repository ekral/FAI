using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Studenti.AvaloniaClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private Student[]? students;

    [ObservableProperty]
    private Student? selectedStudent;


    public MainViewModel()
    {
        Task.Run(LoadStudentAsync);
    }

    private async Task LoadStudentAsync()
    {
        Students = await App.sharedClient.GetFromJsonAsync<Student[]>("/students"); 

        SelectedStudent = Students?.FirstOrDefault();
    }

    public async Task Save()
    {
        if (SelectedStudent is not null)
        {
            await App.sharedClient.PutAsJsonAsync($"/students/{SelectedStudent.StudentId}", SelectedStudent);
        }
    }
}
