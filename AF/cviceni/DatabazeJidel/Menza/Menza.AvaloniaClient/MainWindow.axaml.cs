using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Menza.Models;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Menza.AvaloniaClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnInitialized()
        {
            base.OnInitialized();

            IReadOnlyList<Jidlo>? jidla = await App.Client.GetFromJsonAsync<IReadOnlyList<Jidlo>>("https://localhost:7007/");
        
            if (jidla is not null)
            {
                listBoxJidla.ItemsSource = jidla;
            }
        }
    }
}