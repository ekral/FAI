using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKosk.Models
{
    public record IngredientDTO(int IngredientId, int Quantity);
    public record PizzaDTO(int PizzaId, IngredientDTO[] Ingredients);
    public record OrderDTO(FullfilmentOptionType FullfilmentOption, PizzaDTO[] Pizzas);
}
