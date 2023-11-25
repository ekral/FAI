using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading;

namespace AvaloniaApplication1;

public partial class MojeOknoXaml : Window
{
    int count = 0;

    public MojeOknoXaml()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ++count;
        textBlock.Text = count.ToString();
    }
}