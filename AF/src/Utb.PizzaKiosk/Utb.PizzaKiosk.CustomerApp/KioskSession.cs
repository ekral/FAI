using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.CustomerApp
{
    public class KioskSession
    {
        FullfilmentOptionType FullfilmentOption { get; set; }
        public ShoppingCart Cart { get; set; } = new();
        public Pizza? SelectedPizza { get; set; }

    }
}
