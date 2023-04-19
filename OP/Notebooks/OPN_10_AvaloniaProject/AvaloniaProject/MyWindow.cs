using Avalonia.Controls;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaProject
{
    class MyWindow : Window
    {
        Task<int> DlouhoBeziciMetodaAsync()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                return Random.Shared.Next(0, 100);
            });
        }

        public MyWindow()
        {
            Title = "AvaloniaUI";

            Button button = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Content = "Click"
            };

            TextBlock textBlock = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "0"
            };

            button.Click += async (sender, args) =>
            {
                int vysledek = await DlouhoBeziciMetodaAsync();

                textBlock.Text = vysledek.ToString();
            };

            Content = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Children =
                {
                    textBlock,
                    button
                }
            };
        }
    }
}
