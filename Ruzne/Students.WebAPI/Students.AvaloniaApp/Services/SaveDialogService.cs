using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Services
{
    public class SaveDialogService(TopLevel topLevel) : ISaveDialogService
    {
        private readonly TopLevel topLevel = topLevel;

        public Task<IStorageFile?> ShowAsync()
        {
            var jsonFileType = new FilePickerFileType("JSON File")
            {
                Patterns = ["*.json"],
                MimeTypes = ["application/json"]
            };

            Task<IStorageFile?> file = topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save export file",
                DefaultExtension = "json",
                FileTypeChoices = [jsonFileType]
            });

            return file;
        }
    }
}
