using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Utb.PizzaKiosk.Models
{
    public class PizzaKioskContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; } 
        public DbSet<PizzaIngredient> PizzaIngredients { get; set; } 
        public DbSet<Ingrediet> Ingredients { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderedPizza> OrderedPizzas { get; set; } 
        public DbSet<OrderedIngredient> OrderedIngredients { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, "pizzaKiosk.db");

            SqliteConnectionStringBuilder stringBuilder = new()
            {
                DataSource = filePath
            };
        
            optionsBuilder.UseSqlite(stringBuilder.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
