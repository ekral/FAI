using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.IO;
using System.Threading.Tasks;

namespace Studenti.AvaloniaClient.Services
{
    public class SaveDialogService(TopLevel level) : ISaveDialogService
    {
        private readonly TopLevel level = level;

        public async Task SaveAsync(string json)
        {
            var jsonFileType = new FilePickerFileType("JSON File")
            {
                Patterns = ["*.json"],
                MimeTypes = ["application/json"]
            };

            var options = new FilePickerSaveOptions
            {
                Title = "Save export file",
                DefaultExtension = "json",
                FileTypeChoices = [jsonFileType]
            };

            IStorageFile? file = await level.StorageProvider.SaveFilePickerAsync(options);

            if (file is not null)
            {
                await using var stream = await file.OpenWriteAsync();
                using var streamWriter = new StreamWriter(stream);
                await streamWriter.WriteAsync(json);
            }
        }
    }
}
