using CommunityToolkit.Mvvm.ComponentModel;
using Students.AvaloniaApp.DTOs;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private Student[]? students;

    [ObservableProperty]
    private Student? selectedStudent;

    public string Greeting => "Welcome to Avalonia!";


    public MainViewModel()
    {
        Task.Run(ExampleAsyncMethod);
    }
    private async Task ExampleAsyncMethod()
    {
        Students = await App.sharedClient.GetFromJsonAsync<Student[]>("/students");
        SelectedStudent = Students?.First();
    }

    public async Task Save()
    {
        if (SelectedStudent is not null)
        {
            await App.sharedClient.PutAsJsonAsync($"/students/{SelectedStudent.StudentId}", SelectedStudent);
        }
    }
}


