using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.ViewModels;

public interface IFileService
{
    Task Save(IStorageFile file, string json);
}

public class FileService : IFileService
{
    public async Task Save(IStorageFile file, string json)
    {
        await using var stream = await file.OpenWriteAsync();

        using var streamWriter = new StreamWriter(stream);

        await streamWriter.WriteAsync(json);
    }
}

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private StudentViewModel[]? students;

    [ObservableProperty]
    private StudentViewModel? selectedStudent;

    public event Func<Task<IStorageFile?>>? Interaction;

    private readonly IFileService fileService;

    public MainViewModel(IFileService fileService)
    {
        Task.Run(LoadStudentAsync);
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
        Task<IStorageFile?>? task = Interaction?.Invoke();
       
        if (task is null)
        {
            return;
        }

        IStorageFile? file = await task;

        if(file is null)
        {
            return;
        }

        string json = JsonSerializer.Serialize(Students);

        await fileService.Save(file, json);
    }
}


