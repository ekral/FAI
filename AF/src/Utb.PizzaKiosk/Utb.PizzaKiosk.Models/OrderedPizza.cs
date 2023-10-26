namespace Utb.PizzaKiosk.Models
{
    public class OrderedPizza
    {
        public int Id { get; set; }
        public required int OrderId { get; set; }
        public required string Name { get; set; }
        public required decimal TotalPrice { get; set; }
        public ICollection<OrderedIngredient> OrderedIngredients { get; set; } = new List<OrderedIngredient>();

    }
}
