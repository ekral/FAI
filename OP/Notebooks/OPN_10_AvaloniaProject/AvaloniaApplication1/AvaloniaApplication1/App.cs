using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Microsoft.VisualBasic;

namespace AvaloniaApplication1
{
    public class MyWindow : Window
    {
        public MyWindow()
        {
            
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();


        }
    }

    public class App : Application
    {
        public override void Initialize()
        {
            Styles.Add(new FluentTheme());
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Window window = new MyWindow();
      
            window.Initialized += (sender, e) =>
            {

            };


            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MyWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}
