using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Avalonia;
using System.Security.Cryptography.X509Certificates;
using System.Reactive.Subjects;
using Menza.Models;
using System.Reactive.Linq;
using Avalonia.Data;
using System;
using Avalonia.Markup.Xaml.Templates;
using static System.Formats.Asn1.AsnWriter;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using Avalonia.Controls.Templates;

namespace Menza.Avalonia
{

    public class App : Application
    {
        public static HttpClient Client { get; } = new();

        public override void Initialize()
        {
            Styles.Add(new FluentTheme());
        }

        public override void OnFrameworkInitializationCompleted()
        {
            //Subject<Jidlo> subject = new Subject<Jidlo>();
           
            //TextBlock textBlock = new TextBlock();
            //textBlock.Bind(TextBlock.TextProperty, subject.Select(s => s.Nazev));

            ListBox listBox = new ListBox
            {
                ItemTemplate = new FuncDataTemplate<Jidlo>((value, namescope) =>
                new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding("Nazev"),
                })
            };
            //listBox.ItemsPanel = new ItemsPanelTemplate() { Content = new WrapPanel() };
            //listBox.ItemsSource = jidla;

            //subject.OnNext(new Jidlo() { Id = 1, Cena = 2.0, Nazev = "Dada da" });

            Window window = new Window()
            {
                Content = listBox
            };

            window.Initialized += async (sender, e) =>
            {
                Jidlo[]? jidla = await Client.GetFromJsonAsync<Jidlo[]>("https://localhost:7007/");

                if (jidla is not null)
                {
                    listBox.ItemsSource = jidla;
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
