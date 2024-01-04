using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Styling;
using System.Reactive.Linq;

namespace AvaloniaApplication3
{
    class Vozidlo
    {
        public required string Nazev { get; set; }
        public required decimal Cena { get; set; }
    }

    public class MojeOkno : Window
    {
        public MojeOkno()
        {
            Button button = new Button()
            {
                Content = "Ahoj",
                Background = Brushes.Transparent
            };

            //Styles.Add(new Style(selector => selector.OfType<Button>().Class(":focus"))
            //{
            //    Setters =
            //    {
            //        new Setter(Button.BackgroundProperty, Brushes.Red),
            //    }
            //});

            Content = button;
        }
    }
}
