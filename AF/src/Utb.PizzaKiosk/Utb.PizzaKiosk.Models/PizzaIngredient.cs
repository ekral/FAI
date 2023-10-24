namespace Utb.PizzaKiosk.Models
{
    public class PizzaIngredient
    {
        public required int PizzaId { get; set; }
        public required int IngredientId { get; set; }
        public Ingrediet? Ingrediet { get; set; }
        public required int Quantity { get; set; }
        public required bool Adjustable { get; set; }
    }
}
