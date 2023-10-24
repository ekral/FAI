namespace Utb.PizzaKiosk.Models
{
    public class PizzaIngredient
    {
        public int Id { get; set; }
        public required int PizzaId { get; set; }
        public required int IngredientId { get; set; }
        public required Ingrediet? Ingrediet { get; set; }
        public required int Quantity { get; set; }
        public required bool IsAdjustable { get; set; }
    }
}
