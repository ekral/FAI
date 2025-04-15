using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Students.AvaloniaApp.ViewModels;
using System.IO;

namespace Students.AvaloniaApp.Views;

public partial class MainView : UserControl
{

    public MainView()
    {
        InitializeComponent();
    }

    private async void ExportFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        if (DataContext is not IExportable viewModel)
        {
            return;
        }

        viewModel.ExportToJson();

        string? json = viewModel.Json;

        if (json is null)
        {
            return;
        }

        TopLevel? topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is null)
        {
            return;
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

        if (file is null)
        {
            return;
        }

        await using var stream = await file.OpenWriteAsync();

        using var streamWriter = new StreamWriter(stream);

        await streamWriter.WriteAsync(viewModel.Json);
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}
