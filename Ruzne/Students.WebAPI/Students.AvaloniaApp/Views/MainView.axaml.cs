using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Students.AvaloniaApp.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Views;

public partial class MainView : UserControl
{

    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.Interaction += GetFile;
        }
    }

    private async Task<IStorageFile?> GetFile()
    {
        TopLevel? topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is null)
        {
            return null;
        }

        var jsonFileType = new FilePickerFileType("JSON File")
        {
            Patterns = ["*.json"],
            MimeTypes = ["application/json"]
        };

        IStorageFile? file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save export file",
            DefaultExtension = "json",
            FileTypeChoices = [jsonFileType]
        });

        return file;
    }
}
