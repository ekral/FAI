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
                new Vozidlo() { Nazev = "Cybertruck", Cena = 1300000.0m, ImageUrl = "https://digitalassets.tesla.com/tesla-contents/image/upload/Cybertruck-Specs-Metric-Desktop-Mobile.png"},
                new Vozidlo() { Nazev = "Oktavia RS", Cena = 1100000.0m, ImageUrl = "https://digitalassets.tesla.com/tesla-contents/image/upload/Cybertruck-Specs-Metric-Desktop-Mobile.png"},
                new Vozidlo() { Nazev = "Audi RS5", Cena = 2700000.0m, ImageUrl = "https://digitalassets.tesla.com/tesla-contents/image/upload/Cybertruck-Specs-Metric-Desktop-Mobile.png"}
            };
        }

        public void Pridej()
        {
            Vozidlo nove = new Vozidlo() { Nazev = "Skoda Felicia", Cena = 831000.0m, ImageUrl = "https://eshop.skoda-auto.cz/medias/TN-6U0099300P-645.jpg?context=bWFzdGVyfHJvb3R8MTE4MjR8aW1hZ2UvanBlZ3xoMmQvaDNmLzkxMDI0NDcwODM1NTAvVE5fNlUwMDk5MzAwUF82NDUuanBnfDZlNDZkNGZjOWM1YTMwM2ZmZjg0NTZiZDBlYTQyYmNjYjgxODlmZjk5OTA4YTU4NTNkYWI1YWE4Nzk5N2YyMWI" };
            
            Vozidla.Add(nove);

            VybraneVozidlo = nove;
        }
    }
}
