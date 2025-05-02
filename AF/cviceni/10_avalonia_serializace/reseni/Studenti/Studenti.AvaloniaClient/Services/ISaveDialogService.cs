using System.Threading.Tasks;

namespace Studenti.AvaloniaClient.Services
{
    public interface ISaveDialogService
    {
        Task SaveAsync(string json);
    }
}