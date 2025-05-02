using CommunityToolkit.Mvvm.ComponentModel;
using Studenti.AvaloniaClient.Services;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Studenti.AvaloniaClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private Student[]? students;

    [ObservableProperty]
    private Student? selectedStudent;

    private readonly IStudentService studentService;
    private readonly ISaveDialogService saveDialog;

    public MainViewModel(IStudentService studentService, ISaveDialogService saveDialog)
    {
        this.studentService = studentService;
        this.saveDialog = saveDialog;
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

    public async Task Export()
    {
        if (Students is not null)
        {
            string json = JsonSerializer.Serialize(Students);

            await saveDialog.SaveAsync(json);
        }
    }
}
