namespace Utb.PizzaKiosk.Models
{
    public class Incredience
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Pizza> Pizzas { get; set; } = new List<Pizza>();
    }

}