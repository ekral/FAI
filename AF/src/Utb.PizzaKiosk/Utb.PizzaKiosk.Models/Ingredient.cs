using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required IngredientUnit Unit { get; set; }
        public required int UnitQuantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required string AlergensList { get; set; }
    }
}
