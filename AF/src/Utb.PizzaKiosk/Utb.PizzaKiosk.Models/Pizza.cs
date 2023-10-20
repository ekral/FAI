﻿namespace Utb.PizzaKiosk.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public ICollection<PizzaIngredient>? PizzaIngredients{ get; set; }
    }
}