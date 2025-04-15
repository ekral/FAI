﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase, IExportable
{
    [ObservableProperty]
    private StudentViewModel[]? students;

    [ObservableProperty]
    private StudentViewModel? selectedStudent;

    public string? Json { get; private set; }

    public MainViewModel()
    {
        Task.Run(LoadStudentAsync);
    }

    private async Task LoadStudentAsync()
    {
        Students = await App.sharedClient.GetFromJsonAsync<StudentViewModel[]>("/students");
        SelectedStudent = Students?.First();
    }

    public async Task Save()
    {
        if (SelectedStudent is not null)
        {
            await App.sharedClient.PutAsJsonAsync($"/students/{SelectedStudent.StudentId}", SelectedStudent);
        }
    }

    public void ExportToJson()
    {
        Json = JsonSerializer.Serialize(Students);
    }
}


