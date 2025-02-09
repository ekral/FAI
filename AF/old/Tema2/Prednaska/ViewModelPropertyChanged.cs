using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPrednaska2
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _pocet;

        public int Pocet
        {
            get => _pocet; 
            set 
            { 
                _pocet = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pocet))); 
            }
        }

        public void Zvys()
        {
            ++Pocet;
        }
    }
}
