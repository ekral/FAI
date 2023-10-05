namespace Utb.PizzaKiosk.Models
{

    public abstract class PizzaConfigurationOption
    {
        public int PizzaConfigurationOptionId { get; set; }

        public required string Description { get; set; }
    }
}