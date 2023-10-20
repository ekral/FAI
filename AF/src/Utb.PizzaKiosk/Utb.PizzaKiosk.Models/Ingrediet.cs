using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class Ingrediet
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required IngredientUnit Unit { get; set; }
        public required decimal Price { get; set; }
    }
}
