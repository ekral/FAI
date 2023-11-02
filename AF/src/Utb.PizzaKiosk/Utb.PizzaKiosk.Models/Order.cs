namespace Utb.PizzaKiosk.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public required OrderStatusType OrderStatus { get; set; }
        public required FullfilmentOptionType FullfilmentOption { get; set; }
        public required decimal TotalPrice { get; set; }
        public List<OrderedPizza> OrderedPizzas { get; set; } = new(); // Navigation Property
    }
}
