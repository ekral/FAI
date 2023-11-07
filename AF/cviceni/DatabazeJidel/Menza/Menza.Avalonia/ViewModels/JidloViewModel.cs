using Menza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menza.Avalonia.ViewModels
{
    public class JidloViewModel : ViewModelBase
    {
        private Jidlo jidlo;

        public string Nazev => jidlo.Nazev;

        public JidloViewModel(Jidlo jidlo)
        {
            this.jidlo = jidlo;
        }
    }
}
