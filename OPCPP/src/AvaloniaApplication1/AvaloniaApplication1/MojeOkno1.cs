using Avalonia.Controls;
using Avalonia.Controls.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1
{
    public class MojeOkno1 : Window
    {
        Button button;
        TextBlock textBlock;
        StackPanel panel;

        int count = 0;

        public MojeOkno1()
        {
            button = new Button() { Content = "ahoj" };
            textBlock = new TextBlock() { Text = count.ToString() };
            panel = new StackPanel()
            {
                Children = { button, textBlock }
            };

            button.Click += Button_Click;
            Content = panel;
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ++count;
            textBlock.Text = count.ToString();
        }
    }
}
