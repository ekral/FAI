using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class OrderedIngredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required IngredientUnit Unit { get; set; }
        public int UnitQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int OrderedQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
