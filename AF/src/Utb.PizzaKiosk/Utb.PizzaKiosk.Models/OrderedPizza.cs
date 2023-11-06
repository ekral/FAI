namespace Utb.PizzaKiosk.Models
{
    public class OrderedPizza
    {
        public int OrderedPizzaId { get; set; }
        public required int OrderId { get; set; }
        public required string Name { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalPrice { get; set; }
        public List<OrderedIngredient> OrderedIngredients { get; set; } = new();

    }
}
