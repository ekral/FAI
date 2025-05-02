using Students.AvaloniaApp.Services;
using Students.AvaloniaApp.ViewModels;

class StudentService : IStudentService
{
    private StudentViewModel[]? Students { get; } =
    [
        new StudentViewModel() { StudentId = 1, Jmeno = "Jiri", Studuje = true },
        new StudentViewModel() { StudentId = 2, Jmeno = "Alena", Studuje = false },
        new StudentViewModel() { StudentId = 3, Jmeno = "Samuel", Studuje = true }
    ];

    public Task<StudentViewModel[]?> GetAllStudentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateStudentAsync(StudentViewModel student)
    {
        throw new NotImplementedException();
    }
}

namespace Students.Tests
{
    public class UnitTestMainViewModel
    {
        [Fact]
        public void Test1()
        {
            MainViewModel viewModel = new MainViewModel()
        }
    }
}
