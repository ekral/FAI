namespace Utb.PizzaKiosk.Models
{
    public class OderedPizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal TotalPrice { get; set; }

    }
}
