namespace Utb.PizzaKiosk.Models
{
    public class OrderedPizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal TotalPrice { get; set; }

    }
}
