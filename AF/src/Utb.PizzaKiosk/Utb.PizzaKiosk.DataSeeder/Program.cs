using Utb.PizzaKiosk.Models;

using var context = new PizzaKioskContext();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Ingredients.AddRange(
    new Ingrediet { Id = 1, Name = "Tomato", UnitPrice = 10.0m, Unit = IngredientUnit.Gram, UnitQuantity = 80, AlergensList = "[1,2,3]" },
    new Ingrediet { Id = 2, Name = "Ananas", UnitPrice = 25.0m, Unit = IngredientUnit.Gram, UnitQuantity = 80, AlergensList = "[5,6]" });

context.Pizzas.Add(new Pizza() { Id = 1, Name = "Margherita ", Description = "tomato, mozzarella, cherry rajče, bazalkové pesto", IsAvailable = true, Price = 200.0m, AlergensList = "[1,2,3,4]" });

context.PizzaIngredients.AddRange(
    new PizzaIngredient() { PizzaId = 1, IngredientId = 1, Quantity = 1, Adjustable = false },
    new PizzaIngredient() { PizzaId = 1, IngredientId = 2, Quantity = 0, Adjustable = true });


context.SaveChanges();
