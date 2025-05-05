using Studenti.AvaloniaClient.Services;
using Studenti.AvaloniaClient.ViewModels;
using System.Text.Json;

namespace Studenti.Tests
{
    class MockStudentService : IStudentService
    {
        public Student[] Students { get; } =
        [
            new Student() { StudentId = 1, Jmeno = "Jiri", Studuje = true },
            new Student() { StudentId = 2, Jmeno = "Alena", Studuje = false },
            new Student() { StudentId = 3, Jmeno = "Samuel", Studuje = true }
        ];

        public Task<Student[]?> GetAllStudentsAsync()
        {
            return Task.FromResult(Students)!;
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
        public async Task ExportStudents_ShouldExportStudents()
        {
            MockStudentService studentService = new();
            FakeSaveDialogService dialogService = new();

            MainViewModel viewModel = new(studentService, dialogService);
            
            await viewModel.LoadStudentAsync();

            Assert.NotNull(viewModel.Students);
            Assert.NotNull(viewModel.SelectedStudent);

            await viewModel.ExportAsync();

            string expected = JsonSerializer.Serialize(studentService.Students);
            
            Assert.Equal(expected, dialogService.Json);
        }

        [Fact]
        public async Task UpdateStudent_ShouldUpdateStudentDetails()
        {
            MockStudentService studentService = new();
            FakeSaveDialogService saveDialog = new();

            MainViewModel viewModel = new(studentService, saveDialog);

            await viewModel.LoadStudentAsync();

            Assert.NotNull(viewModel.Students);
            Assert.NotNull(viewModel.SelectedStudent);

            viewModel.SelectedStudent.Jmeno = "UpdatedName";
            viewModel.SelectedStudent.Studuje = false;

            await viewModel.SaveAsync();

            Student? updatedStudent = studentService.Students.FirstOrDefault(s => s.StudentId == viewModel.SelectedStudent.StudentId);

            Assert.NotNull(updatedStudent);

            Assert.Equal("UpdatedName", updatedStudent.Jmeno);
            Assert.False(updatedStudent.Studuje);
        }

    }
}
