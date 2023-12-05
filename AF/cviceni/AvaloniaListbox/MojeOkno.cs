using Avalonia.Controls;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Data;
using Avalonia;
using Avalonia.Controls.Templates;

namespace AvaloniaApplication3
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? name = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);

            if (EqualityComparer<T>.Default.Equals(field, newValue)) return false;

            field = newValue;
            OnPropertyChanged(name);

            return true;
        }
    }

    class Vozidlo
    {
        public required string Nazev { get; set; }
        public required decimal Cena { get; set; }
    }


    class KatalogVozidel : ViewModelBase
    {
        private Vozidlo? vybraneVozidlo = null;

        public ObservableCollection<Vozidlo> Vozidla { get; set; }
        public Vozidlo? VybraneVozidlo { get => vybraneVozidlo; set => SetProperty(ref vybraneVozidlo, value); }
        public KatalogVozidel()
        {
            Vozidla = new ObservableCollection<Vozidlo>()
            {
                new Vozidlo() { Nazev = "Cybertruck", Cena = 1300000.0m},
                new Vozidlo() { Nazev = "Oktavia RS", Cena = 1100000.0m},
                new Vozidlo() { Nazev = "Audi RS5", Cena = 2700000.0m}
            };
        }

        public void Pridej()
        {
            Vozidlo nove = new Vozidlo() { Nazev = "Mazda MX5", Cena = 831000.0m };
            Vozidla.Add(nove);
        }
    }

    public class MojeOkno : Window
    {
        public MojeOkno()
        {
            DataContext = new KatalogVozidel();

            ListBox listBox = new ListBox();
            listBox.Bind(ListBox.ItemsSourceProperty, new Binding("Vozidla"));
            listBox.Bind(ListBox.SelectedItemProperty, new Binding("VybraneVozidlo"));

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

            Button buttonNoveVozidlo = new Button()
            {
                Content = "Pridej nove vozidlo"
            };

            buttonNoveVozidlo.Bind(Button.CommandProperty, new Binding("Pridej"));

            TextBlock textBlockNazevVybraneho = new TextBlock();
            textBlockNazevVybraneho.Bind(TextBlock.TextProperty, new Binding("VybraneVozidlo.Nazev"));

            StackPanel panel = new StackPanel()
            {
                Children = { buttonNoveVozidlo, listBox, textBlockNazevVybraneho }
            };

            Content = panel;
            
        }
    }
}
