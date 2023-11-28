using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;

namespace AvaloniaAplikace
{
    public class MojeOkno : Window
    {
        int count = 0;
    
        public MojeOkno()
        {
    
            TextBlock textBlockPopis = new TextBlock()
            {
                FontSize = 18.0,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5, 0),
                Text = "Alignment, Margin and Padding Sample"
            };
            Button button = new Button() 
            { 
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(20),
                Content = "Tlacitko" ,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
    
            TextBlock textBlock = new TextBlock() 
            {
                FontSize = 28.0,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(20),
                Text = count.ToString() 
            };
    
            button.Click += (sender, e) =>
            {
                ++count;
                textBlock.Text = count.ToString();
                textBlock.FontSize += 1.0;
    
            };
    
            StackPanel stackPanel = new StackPanel()
            {
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
    
            stackPanel.Children.Add(textBlockPopis);
            stackPanel.Children.Add(button);
            stackPanel.Children.Add(textBlock);
    
            Border border = new Border()
            {
                Background = Brushes.LightBlue,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Padding = new Thickness(15),
                Child = stackPanel
            };
    
            Content = border;
        }
    }
}

