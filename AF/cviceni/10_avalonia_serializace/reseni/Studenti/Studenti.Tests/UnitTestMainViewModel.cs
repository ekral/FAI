using NSubstitute;
using Studenti.AvaloniaClient.Services;
using Studenti.AvaloniaClient.ViewModels;
using System.Text.Json;

namespace Studenti.Tests
{
    public class UnitTestMainViewModel
    {
        [Fact]
        public async Task ExportStudents_ShouldExportStudents()
        {
            Student[] data = [new Student() { StudentId = 1, Jmeno = "Jiri", Studuje = false }, new Student() { StudentId = 2, Jmeno = "Karel", Studuje = true }];

            IStudentService studentService = Substitute.For<IStudentService>();
            studentService.GetAllStudentsAsync().Returns(data);

            var dialogService = Substitute.For<ISaveDialogService>();
            dialogService.SaveAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            MainViewModel viewModel = new(studentService, dialogService);

            await viewModel.LoadStudentAsync();
            await viewModel.ExportAsync();

            string expected = JsonSerializer.Serialize(data);
            
            Assert.NotNull(viewModel.Students);
            Assert.NotNull(viewModel.SelectedStudent);

            await dialogService.Received().SaveAsync(expected); // Otestuje, ze se zavola metoda SaveAsync s danym argumentem
        }

        [Fact]
        public async Task UpdateStudent_ShouldUpdateStudentDetails()
        {
            Student[] data = [new Student() { StudentId = 1, Jmeno = "Jiri", Studuje = false }, new Student() { StudentId = 2, Jmeno = "Karel", Studuje = true }];

            IStudentService studentService = Substitute.For<IStudentService>();

            studentService.GetAllStudentsAsync().Returns(data);
            Student? updatedStudent = null;
            studentService.UpdateStudentAsync(Arg.Do<Student>(student => updatedStudent = student)).Returns(Task.CompletedTask);

            var dialogService = Substitute.For<ISaveDialogService>();

            MainViewModel viewModel = new(studentService, dialogService);

            await viewModel.LoadStudentAsync();

            Assert.NotNull(viewModel.Students);
            Assert.NotNull(viewModel.SelectedStudent);

            viewModel.SelectedStudent.Jmeno = "UpdatedName";
            viewModel.SelectedStudent.Studuje = false;

            await viewModel.SaveAsync();

            Assert.NotNull(updatedStudent);

            Assert.Equal("UpdatedName", updatedStudent.Jmeno);
            Assert.False(updatedStudent.Studuje);
        }
    }
}
