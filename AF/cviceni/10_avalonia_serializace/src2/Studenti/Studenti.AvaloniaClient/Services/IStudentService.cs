using Studenti.AvaloniaClient.ViewModels;
using System.Threading.Tasks;

namespace Studenti.AvaloniaClient.Services
{
    public interface IStudentService
    {
        Task<Student[]?> GetAllStudentsAsync();
        Task UpdateStudentAsync(Student student);
    }
}