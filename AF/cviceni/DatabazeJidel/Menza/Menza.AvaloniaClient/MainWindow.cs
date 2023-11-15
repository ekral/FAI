using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Menza.Models;
using Avalonia.Media;
using Avalonia.Layout;
using System.Collections.Generic;
using System.Net.Http.Json;
using Avalonia;
using Avalonia.Styling;

namespace Menza.AvaloniaClient
{
    class MainWindow : Window
    {
        readonly ListBox listBox;

        public MainWindow()
        {

            listBox = new ListBox()
            {
              
                HorizontalAlignment = HorizontalAlignment.Stretch,
                ItemsPanel = new FuncTemplate<Panel?>(() =>
                    new WrapPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Orientation = Orientation.Horizontal,
                    }),
                ItemTemplate = new FuncDataTemplate<Jidlo>((jidlo, scope) =>
                    new Border()
                    {
                        Width = 120,
                        Height = 120,
                        Background = Brushes.Red,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(2.0),
                        CornerRadius = new CornerRadius(2.0),
                        Padding = new Thickness(4.0),
                        Child = new StackPanel()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Children =
                            {
                                new TextBlock()
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Text = jidlo.Nazev
                                },
                                new TextBlock()
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Text = jidlo.Cena.ToString()
                                },
                            }
                        }
                    })
            };

            
           
            Title = "Menza";
            Content = listBox;
        }

        protected override async void OnInitialized()
        {
            base.OnInitialized();

            IReadOnlyList<Jidlo>? jidla = await App.Client.GetFromJsonAsync<IReadOnlyList<Jidlo>>("https://localhost:7007/");

            if (jidla is not null)
            {
                listBox.ItemsSource = jidla;
            }
        }
    }
}