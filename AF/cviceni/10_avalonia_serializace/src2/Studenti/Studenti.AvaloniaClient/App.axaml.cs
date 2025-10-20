using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Studenti.AvaloniaClient.ViewModels;
using Studenti.AvaloniaClient.Views;
using System.Net.Http;
using System;
using Avalonia.Controls;
using Studenti.AvaloniaClient.Services;
using System.Threading.Tasks;

namespace Studenti.AvaloniaClient;

public partial class App : Application
{
    public static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://localhost:7266")
    };

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();

            TopLevel topLevel = TopLevel.GetTopLevel(desktop.MainWindow) ?? throw new NullReferenceException();

            MainViewModel vm = new(new StudentService(sharedClient), new SaveDialogService(topLevel));

            desktop.MainWindow.DataContext = vm;

            desktop.MainWindow.Loaded += async (sender, e) => await vm.LoadStudentAsync();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();

            TopLevel topLevel = TopLevel.GetTopLevel(singleViewPlatform.MainView) ?? throw new NullReferenceException();

            MainViewModel vm = new(new StudentService(sharedClient), new SaveDialogService(topLevel));

            singleViewPlatform.MainView.DataContext = vm;

            Task.Run(vm.LoadStudentAsync);

            singleViewPlatform.MainView.Loaded += async (sender, e) => await vm.LoadStudentAsync();
        }

        base.OnFrameworkInitializationCompleted();
    }

    
}
