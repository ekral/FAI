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
    public partial class ViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _pocet = 7;
       
        public void Zvys()
        {
            ++Pocet;
        }
    }
}
