using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class OderedPizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal TotalPrice { get; set; }

    }
}
