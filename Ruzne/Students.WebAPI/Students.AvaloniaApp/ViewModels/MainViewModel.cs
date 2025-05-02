using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Students.AvaloniaApp.Services;
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

    private readonly IStudentService studentService;
    private readonly ISaveDialogService saveDialog;
    private readonly IFileService fileService;

    public MainViewModel(IStudentService studentService, ISaveDialogService saveDialog, IFileService fileService)
    {
        this.studentService = studentService;
        this.saveDialog = saveDialog;
        this.fileService = fileService;
    }

    public async Task LoadStudentAsync()
    {
        Students = await studentService.GetAllStudentsAsync();

        SelectedStudent = Students?.FirstOrDefault();
    }

    public async Task Save()
    {
        if (SelectedStudent is not null)
        {
            await studentService.UpdateStudentAsync(SelectedStudent);
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


