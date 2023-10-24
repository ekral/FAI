using Utb.PizzaKiosk.Models;

using var context = new PizzaKioskContext();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Pizzas.Add(new Pizza() { Name = "Margherita ", Description = "tomato, mozzarella, cherry rajče, bazalkové pesto", IsAvailable = true, Price = 200, AlergensList = "[1,2,3,4]" });

context.SaveChanges();
