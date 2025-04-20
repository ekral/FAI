using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Students.AvaloniaApp.Services;
using Students.AvaloniaApp.Views;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private StudentViewModel[]? students;

    [ObservableProperty]
    private StudentViewModel? selectedStudent;

    private readonly ISaveDialogService saveDialog;
    private readonly IFileService fileService;

    public MainViewModel(ISaveDialogService saveDialog, IFileService fileService)
    {
        Task.Run(LoadStudentAsync);
        this.saveDialog = saveDialog;
        this.fileService = fileService;
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

    public async Task ExportToJson()
    {
        IStorageFile? storageFile = await saveDialog.ShowAsync();

        if (storageFile is not null)
        {
            string json = JsonSerializer.Serialize(Students);

            await fileService.SaveAsync(storageFile, json);
        }
    }
}


