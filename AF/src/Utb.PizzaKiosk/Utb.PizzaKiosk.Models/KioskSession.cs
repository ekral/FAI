namespace Utb.PizzaKiosk.Models
{
    public class KioskSession
    {
        FullfilmentOptionType FullfilmentOption { get; set; }
        public ShoppingCart Cart { get; set; } = new();
        public Pizza? SelectedPizza { get; set; }

    }
}
