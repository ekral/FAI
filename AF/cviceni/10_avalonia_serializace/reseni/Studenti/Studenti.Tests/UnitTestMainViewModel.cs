using Studenti.AvaloniaClient.Services;
using Studenti.AvaloniaClient.ViewModels;

namespace Studenti.Tests
{
    class MockStudentService : IStudentService
    {
        private Student[]? Students { get; } =
        [
            new Student() { StudentId = 1, Jmeno = "Jiri", Studuje = true },
            new Student() { StudentId = 2, Jmeno = "Alena", Studuje = false },
            new Student() { StudentId = 3, Jmeno = "Samuel", Studuje = true }
        ];

        public Task<Student[]?> GetAllStudentsAsync()
        {
            return Task.FromResult(Students);
        }

        public Task UpdateStudentAsync(Student student)
        {
            Student? studentToChange = Students?.FirstOrDefault(s => s.StudentId == student.StudentId);

            if (studentToChange is not null)
            {
                studentToChange.Jmeno = student.Jmeno;
                studentToChange.Studuje = student.Studuje;
            }

            return Task.CompletedTask;
        }
    }

    class FakeSaveDialogService : ISaveDialogService
    {
        public string? Json { get; private set; }
        
        public Task SaveAsync(string json)
        {
            Json = json;

            return Task.CompletedTask;
        }
    }

    public class UnitTestMainViewModel
    {
        [Fact]
        public void Test1()
        {
            MainViewModel viewModel = new MainViewModel(new MockStudentService(), new FakeSaveDialogService());

            
        }
    }
}
