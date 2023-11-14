using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Menza.Models;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Menza.AvaloniaClient
{
    class MainWindow : Window
    {
        readonly ListBox listBox;

        public MainWindow()
        {
            listBox = new ListBox()
            {
                ItemsPanel = new FuncTemplate<Panel?>(() => new WrapPanel() { Orientation = Avalonia.Layout.Orientation.Vertical}),
                ItemTemplate = new FuncDataTemplate<Jidlo>((jidlo, scope) =>
                
                    new TextBlock() { Text = jidlo.Nazev })
            };

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