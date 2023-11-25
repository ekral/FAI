namespace Utb.PizzaKiosk.Models
{
    public class PizzaStyle
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public ICollection<Pizza> Pizzas { get; set; } = new List<Pizza>();
    }

}