using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System.Collections.Generic;

namespace AvaloniaApplication5
{

    public class MojeOkno : Window
    {
        public class ViewModel
        {
            public List<int> Cisla { get; } = new() { 1, 2, 3 };
        }

        public MojeOkno()
        {
            DataContext = new ViewModel();

            ListBox listBox = new ListBox();

            listBox.Bind(ListBox.ItemsSourceProperty, new Binding("Cisla"));

            Content = listBox;
        }

        private void Metoda(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
