using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Models.DTO
{
    public record PizzaDTO(int PizzaId, IngredientDTO[] Ingredients);
}
