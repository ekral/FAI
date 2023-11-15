using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Menza.Models;
using Avalonia.Media;
using Avalonia.Layout;
using System.Collections.Generic;
using Avalonia;

namespace Menza.AvaloniaClient
{
    class MainWindow : Window
    {
        readonly ListBox listBox;
        private readonly IMenzaService menzaService;

        public MainWindow(IMenzaService menzaService)
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

            Content = listBox;
            this.menzaService = menzaService;
        }

        protected override async void OnInitialized()
        {
            base.OnInitialized();

            IReadOnlyList<Jidlo> jidla = await menzaService.GetJidlaAsync();

            listBox.ItemsSource = jidla;

        }
    }
}