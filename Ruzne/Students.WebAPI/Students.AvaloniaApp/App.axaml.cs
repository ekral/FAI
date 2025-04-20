using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Students.AvaloniaApp.ViewModels;
using Students.AvaloniaApp.Views;
using System.Net.Http;
using System;
using Avalonia.Controls;
using Students.AvaloniaApp.Services;

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

        StudentService studentService = new StudentService(sharedClient);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            
            TopLevel topLevel = TopLevel.GetTopLevel(desktop.MainWindow) ?? throw new NullReferenceException();

            desktop.MainWindow.DataContext = new MainViewModel(studentService, new SaveDialogService(topLevel), new FileService());
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();

            TopLevel topLevel = TopLevel.GetTopLevel(singleViewPlatform.MainView) ?? throw new NullReferenceException();

            singleViewPlatform.MainView.DataContext = new MainViewModel(studentService, new SaveDialogService(topLevel), new FileService());
        }

        base.OnFrameworkInitializationCompleted();
    }
}
