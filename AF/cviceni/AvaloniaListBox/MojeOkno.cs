using Avalonia.Controls;
using Avalonia.Data;
using Avalonia;
using Avalonia.Controls.Templates;

namespace AvaloniaApplication3
{

    public class MojeOkno : Window
    {
         public MojeOkno()
        {
            DataContext = new KatalogVozidel();

            Button buttonNoveVozidlo = new Button()
            {
                Content = "Pridej nove vozidlo"
            };

            ListBox listBox = new ListBox();
           
            listBox.ItemTemplate = new FuncDataTemplate<Vozidlo>((vozidlo, scope) =>
            {
                TextBlock textBlockNazev = new TextBlock()
                {
                    Text = vozidlo.Nazev
                };

                TextBlock textBlockCena = new TextBlock()
                {
                    Text = vozidlo.Cena.ToString()
                };

                StackPanel stackPanel = new StackPanel()
                {
                    Children = { textBlockNazev, textBlockCena }
                };

                return stackPanel;

            });

            TextBlock textBlockNazevVybraneho = new TextBlock();

            StackPanel stackPanel = new StackPanel()
            {
                Children = { buttonNoveVozidlo, listBox, textBlockNazevVybraneho }
            };

            buttonNoveVozidlo.Bind(Button.CommandProperty, new Binding("Pridej"));
            listBox.Bind(ListBox.ItemsSourceProperty, new Binding("Vozidla"));
            listBox.Bind(ListBox.SelectedItemProperty, new Binding("VybraneVozidlo"));
            textBlockNazevVybraneho.Bind(TextBlock.TextProperty, new Binding("VybraneVozidlo.Nazev"));


            Content = stackPanel;
            
        }
    }
}
