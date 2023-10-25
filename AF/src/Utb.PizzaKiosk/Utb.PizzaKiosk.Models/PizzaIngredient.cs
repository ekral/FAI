namespace Utb.PizzaKiosk.Models
{
    public class PizzaIngredient
    {
        public required int PizzaId { get; set; }
        public required int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
        public required int MinimalQuantity { get; set; }
        public required int DefaultQuantity { get; set; }
    }
}
