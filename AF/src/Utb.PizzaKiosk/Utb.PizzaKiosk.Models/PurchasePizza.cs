namespace Utb.PizzaKiosk.Models
{
    public class PurchasePizza
    {
        public int Id { get; set; }
        public required int OrderId { get; set; }
        public required string Name { get; set; }
        public required decimal TotalPrice { get; set; }

    }
}
