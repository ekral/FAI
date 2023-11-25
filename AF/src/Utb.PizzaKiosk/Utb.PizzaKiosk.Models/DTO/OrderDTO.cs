namespace Utb.PizzaKiosk.Models.DTO
{
    public record OrderDTO(FullfilmentOptionType FullfilmentOption, PizzaDTO[] Pizzas);
}
