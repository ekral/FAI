using Avalonia.Platform.Storage;
using System.IO;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Services;

public class FileService : IFileService
{
    public async Task SaveAsync(IStorageFile file, string json)
    {
        await using var stream = await file.OpenWriteAsync();

        using var streamWriter = new StreamWriter(stream);

        await streamWriter.WriteAsync(json);
    }
}


