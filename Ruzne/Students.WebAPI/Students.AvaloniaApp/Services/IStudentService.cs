using Students.AvaloniaApp.ViewModels;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Services
{
    public interface IStudentService
    {
        Task<StudentViewModel[]?> GetAllStudentsAsync();
        Task UpdateStudentAsync(StudentViewModel student);
    }
}