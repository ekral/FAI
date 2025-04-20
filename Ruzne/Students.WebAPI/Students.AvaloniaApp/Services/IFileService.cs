using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Services;

public interface IFileService
{
    Task SaveAsync(IStorageFile file, string json);
}


