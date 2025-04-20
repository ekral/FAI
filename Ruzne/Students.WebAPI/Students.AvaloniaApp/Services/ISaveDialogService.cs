using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Services
{
    public interface ISaveDialogService
    {
        Task<IStorageFile?> ShowAsync();
    }
}