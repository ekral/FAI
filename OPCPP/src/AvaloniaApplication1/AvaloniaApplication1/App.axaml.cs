using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Cryptography.X509Certificates;

namespace AvaloniaApplication1
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Subject<int> stav = new Subject<int>();

            TextBlock textBlock = new TextBlock();
            textBlock.Bind(TextBlock.TextProperty, stav.Select(x => x.ToString()));
            stav.OnNext(0);

            Button button = new Button() { Content = "Tlacitko" };

            button.Click += (sender, args) =>
            {
                stav.OnNext(1);
            };

            Window window = new Window()
            {
                Title = "Moje okno",
                Content = new StackPanel()
                {
                    Children =
                    {
                        button,
                        textBlock
                    }
                }
            };




            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}