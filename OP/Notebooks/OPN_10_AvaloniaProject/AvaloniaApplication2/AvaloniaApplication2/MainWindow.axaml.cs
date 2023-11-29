using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Themes.Fluent;
using System;
using System.Linq;

namespace AvaloniaApplication2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
        
            InitializeComponent();

            var color = Application.Current.Resources["SystemAccentColorDark1"];



        }
    }
}