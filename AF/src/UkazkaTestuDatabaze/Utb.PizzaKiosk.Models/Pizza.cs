namespace Utb.PizzaKiosk.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public int PizzaStyleId { get; set; } // cizi klic
        public PizzaStyle? PizzaStyle { get; set; } // Navigation property
        public ICollection<Incredience>? Incrediences { get; set; }
    }
}