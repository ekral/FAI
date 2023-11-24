using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;
using Avalonia.Data;
using System.ComponentModel;

namespace AvaloniaAplikace
{
    public class ViewModelObjednavka : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public required string NazevProduktu { get; set; }
        public decimal Cena { get; set; }
        public int Pocet { get; set; }

        private decimal cenaCelkem;
        public decimal CenaCelkem 
        {
            get => cenaCelkem; 
            set
            {
                cenaCelkem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CenaCelkem)));
            }
        }

        public void SpocitejCenuCelkem()
        {
            CenaCelkem = Pocet * Cena;
        }
    }

    public class MojeOkno : Window
    {
        public MojeOkno()
        {
            DataContext = new ViewModelObjednavka() { NazevProduktu = "Tesla 3", Cena = 1100000.0m, Pocet = 1 };

            TextBlock textBlockNazevProduktu = new TextBlock();
            textBlockNazevProduktu.Bind(TextBlock.TextProperty, new Binding("NazevProduktu"));

            TextBlock textBlockCena = new TextBlock();
            textBlockCena.Bind(TextBlock.TextProperty, new Binding("Cena"));

            TextBlock textBlockCenaCelkem = new TextBlock()
            {
                Background = Brushes.LightBlue
            };

            textBlockCenaCelkem.Bind(TextBlock.TextProperty, new Binding("CenaCelkem"));

            NumericUpDown numericUpDown = new NumericUpDown()
            {
                HorizontalAlignment =  HorizontalAlignment.Left,
                Minimum = 1,
                Maximum = 10,
                Increment = 1
            };

            numericUpDown.Bind(NumericUpDown.ValueProperty, new Binding("Pocet", BindingMode.TwoWay));

            Button buttonSpocitej = new Button()
            {
                Content = "Cena celkem"
            };

            buttonSpocitej.Bind(Button.CommandProperty, new Binding("SpocitejCenuCelkem"));

            StackPanel panel = new StackPanel();

            panel.Children.Add(textBlockNazevProduktu);
            panel.Children.Add(textBlockCena);
            panel.Children.Add(textBlockCenaCelkem);
            panel.Children.Add(numericUpDown);
            panel.Children.Add(buttonSpocitej);

            Content = panel;
        }
    }
}
