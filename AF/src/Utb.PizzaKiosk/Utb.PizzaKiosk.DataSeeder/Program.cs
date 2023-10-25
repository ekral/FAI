using Utb.PizzaKiosk.Models;

using var context = new PizzaKioskContext();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Pizzas.AddRange(
    new Pizza() { Id = 1, Name = "Hot Dog Pizza", Description = "tomato, mozzarella, cibule, párečky, nakládaná okurka, medová hořčice", IsAvailable = true, Price = 250.0m, AlergensList = "[1,2,3,4]" },
    new Pizza() { Id = 2, Name = "Margherita", Description = "tomato, mozzarella, cherry rajče, bazalkové pesto", IsAvailable = true, Price = 200.0m, AlergensList = "[1,2,3,4]" },
    new Pizza() { Id = 3, Name = "Sýrová", Description = "tomato, mozzarella, cherry rajče, bazalkové pesto", IsAvailable = true, Price = 200.0m, AlergensList = "[1,2,3,4]" }
    );

context.Ingredients.AddRange(
    new Ingredient { Id = 1, Name = "Nakládaná okurka", QuantityDescription = "15 g", UnitPrice = 15.0m, AlergensList = "[1,2,3]" },
    new Ingredient { Id = 2, Name = "Ananas", QuantityDescription = "80 g", UnitPrice = 25.0m, AlergensList = "[5,6]" },
    new Ingredient { Id = 3, Name = "Mozzarela", QuantityDescription = "80 g", UnitPrice = 25.0m, AlergensList = "[5,6]" }
    );

context.PizzaIngredients.AddRange(
    new PizzaIngredient() { PizzaId = 1, IngredientId = 1, MinimalQuantity = 0, DefaultQuantity = 1 },
    new PizzaIngredient() { PizzaId = 1, IngredientId = 2, MinimalQuantity = 0, DefaultQuantity = 0},
    new PizzaIngredient() { PizzaId = 2, IngredientId = 1, MinimalQuantity = 0, DefaultQuantity = 0},
    new PizzaIngredient() { PizzaId = 2, IngredientId = 2, MinimalQuantity = 0, DefaultQuantity = 0},
    new PizzaIngredient() { PizzaId = 3, IngredientId = 1, MinimalQuantity = 0, DefaultQuantity = 0 },
    new PizzaIngredient() { PizzaId = 3, IngredientId = 2, MinimalQuantity = 0, DefaultQuantity = 0 },
    new PizzaIngredient() { PizzaId = 3, IngredientId = 3, MinimalQuantity = 1, DefaultQuantity = 1 }
    );


context.SaveChanges();
