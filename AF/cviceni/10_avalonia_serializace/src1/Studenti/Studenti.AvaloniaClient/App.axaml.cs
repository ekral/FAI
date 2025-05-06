using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Studenti.AvaloniaClient.ViewModels;
using Studenti.AvaloniaClient.Views;
using System.Net.Http;
using System;

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
            MainViewModel vm = new(); 

            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };

            desktop.MainWindow.Loaded += async (sender, e) => await vm.LoadStudentAsync();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            MainViewModel vm = new();

            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };

            singleViewPlatform.MainView.Loaded += async (sender, e) => await vm.LoadStudentAsync();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
