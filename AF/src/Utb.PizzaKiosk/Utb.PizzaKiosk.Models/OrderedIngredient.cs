﻿namespace Utb.PizzaKiosk.Models
{
    public class OrderedIngredient
    {
        public int Id { get; set; }
        public required int PizzaId { get; set; }
        public required string Name { get; set; }
        public required IngredientUnit Unit { get; set; }
        public required int UnitQuantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int OrderedQuantity { get; set; }
        public required decimal TotalPrice { get; set; }
    }
}