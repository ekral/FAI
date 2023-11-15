using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Menza.AvaloniaClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(p => new HttpClient() { BaseAddress = new System.Uri("https://localhost:7007/") });
            serviceCollection.AddSingleton<IMenzaService, MenzaServiceMock>();
            serviceCollection.AddTransient<MainWindow>();
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = serviceProvider.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}