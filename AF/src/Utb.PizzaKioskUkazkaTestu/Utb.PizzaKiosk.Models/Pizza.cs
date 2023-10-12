using System.Collections.Generic;

namespace Utb.PizzaKiosk.Models
{
    public class Pizza
    {
        public int Id { get; set; } // Primární klíč dle jmenných konvencí
        public required string Jmeno { get; set; }
        public required string Description { get; set; }
        public required double Price { get; set; }
    }
}