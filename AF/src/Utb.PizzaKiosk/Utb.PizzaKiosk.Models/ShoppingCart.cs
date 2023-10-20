using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class ShoppingCart
    {
        public CartStatusType Status { get; set; }
        public List<Pizza> CartPizzas { get; set; } = new();
    }
}
