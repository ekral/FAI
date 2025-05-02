using Students.AvaloniaApp.Services;
using Students.AvaloniaApp.ViewModels;

class StudentService : IStudentService
{
    private StudentViewModel[] Students { get; } =
        [
            ne
        ]
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
