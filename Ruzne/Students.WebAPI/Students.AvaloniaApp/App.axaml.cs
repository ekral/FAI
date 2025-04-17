using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Students.AvaloniaApp.ViewModels;
using Students.AvaloniaApp.Views;
using System.Net.Http;
using System;
using Avalonia.Controls;

namespace Students.AvaloniaApp;

public partial class App : Application
{
    public static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://localhost:7042")
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
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(new FileService())
            };

            TopLevel? topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(new FileService())
            };

            TopLevel? topLevel = TopLevel.GetTopLevel(singleViewPlatform.MainView); // OK
        }

        base.OnFrameworkInitializationCompleted();
    }
}
