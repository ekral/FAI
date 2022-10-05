using System.ComponentModel;

namespace Prednaska3
{
    public class ViewModelStudent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IModel model;

        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }

        private double _body;
        public double Body 
        { 
            get => _body; 
            set
            {
                _body = value;
                OvereniVysledku();
            }
        }

        private string? _vysledek;

        public string? Vysledek 
        { 
            get => _vysledek; 
            set
            {
                _vysledek = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vysledek)));
            }
        }

        public ViewModelStudent(IModel model)
        {
            this.model = model;
            Jmeno = "Jiri";
            Prijmeni = "Zeleny";
            Body = 30.0;
        }


        public void OvereniVysledku()
        {
            Vysledek = model.UlozAOverVysledek(Body) ? "Prosel" : "Neprosel";
        }
    }
}
