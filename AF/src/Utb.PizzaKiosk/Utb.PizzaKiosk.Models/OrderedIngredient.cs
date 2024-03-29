﻿namespace Utb.PizzaKiosk.Models
{
    public class OrderedIngredient
    {
        public int OrderedIngredientId { get; set; }
        public required int OrderedPizzaId { get; set; }
        public required string Name { get; set; }
        public required string QuantityDescription { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int FreeQuantity { get; set; }
        public required int PaidQuantity { get; set; }
        public required decimal TotalPrice { get; set; }
    }
}
