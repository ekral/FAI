using System.Collections.ObjectModel;

namespace AvaloniaApplication3
{
    class KatalogVozidel : ViewModelBase
    {
        private Vozidlo? vybraneVozidlo = null;

        public ObservableCollection<Vozidlo> Vozidla { get; }
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
}
