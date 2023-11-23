using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvaloniaAplikace
{
    public class MojeOkno : Window
    {
        private int count = 0;

        public MojeOkno()
        {
            // Pridejte do aplikace dva textboxy
            // A tlacitko Secti
            // Po stlaceni tlacitka prevedte text z textboxu na int, sectete oba inty a vysledek zobrazte v textbloku
            TextBox textBox = new TextBox();

            TextBlock textBlock = new TextBlock()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Text = count.ToString()
            };

            Button button = new Button()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Margin = new Thickness(10),
                Content = "Tlacitko"
            };

            button.Click += (sender, e) => 
            { 
                ++count; 
                textBlock.Text = count.ToString(); 
            };

            Border border = new Border
            {
                Background = Brushes.LightBlue,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Padding = new Thickness(15),
                CornerRadius = new CornerRadius(8)
            };

            StackPanel stackPanel = new StackPanel() { Background = Brushes.Black };
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(button);
            stackPanel.Children.Add(textBox);

            border.Child = stackPanel;

            Content = border;
        }
    }
}
