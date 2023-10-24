using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Utb.PizzaKiosk.Models
{
    public class PizzaKioskContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; } 
        public DbSet<PizzaIngredient> PizzaIngredients { get; set; } 
        public DbSet<Ingredient> Ingredients { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderedPizza> OrderedPizzas { get; set; } 
        public DbSet<OrderedIngredient> OrderedIngredients { get; set; }

        public PizzaKioskContext()
        {
            
        }

        public PizzaKioskContext(DbContextOptions<PizzaKioskContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured)
            {
                return;
            }

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
            modelBuilder.Entity<PizzaIngredient>().HasKey(pi => new { pi.PizzaId, pi.IngredientId });
        }
    }
}
