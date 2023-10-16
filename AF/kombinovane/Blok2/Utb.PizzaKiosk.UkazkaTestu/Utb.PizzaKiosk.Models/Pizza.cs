namespace Utb.PizzaKiosk.Models
{
    
    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required double Cena { get; set; }
        public int PizzaStyleId { get; set; } // cizi klic
        public PizzaStyle? PizzaStyle { get; set; } // Navigation property
        public ICollection<Incredience>? Incrediences { get; set; }
    }

    public class Incredience
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Pizza>? Pizzas { get; set; }
    }

    public class PizzaIncredience
    {
        public required int PizzaId { get; set; }
        public required int IncredienceId { get; set; }
    }

    public class PizzaStyle
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public ICollection<Pizza>? Pizzas { get; set; } // Collection Navigation property
    }

}