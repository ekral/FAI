using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Students.AvaloniaApp.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace Students.AvaloniaApp.Views;

public class Message : RequestMessage<Task<IStorageFile?>>
{
}

public partial class MainView : UserControl
{

    public MainView()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<MainView, Message>(this, async (r, m) =>
        {
            TopLevel? topLevel = TopLevel.GetTopLevel(this);

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

            // Assume that "CurrentUser" is a private member in our viewmodel.
            // As before, we're accessing it through the recipient passed as
            // input to the handler, to avoid capturing "this" in the delegate.
            m.Reply(file);
        });
    }

    
}
