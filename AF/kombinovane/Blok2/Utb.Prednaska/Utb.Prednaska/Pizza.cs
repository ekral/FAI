using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Utb.Prednaska
{
    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public int PizzaStyleId { get; set; } // cizi klic
        public PizzaStyle? PizzaStyle { get; set; } // Navigation property
        public ICollection<Incredience>? Incrediences { get; set; }
    }

    public class Incredience
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Pizza>? Pizzas { get; set; }
    }

    public class PizzaIncredience
    {
        public required int PizzaId { get; set; }
        public required int IncredienceId { get; set; }
    }

    public class PizzaStyle
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public ICollection<Pizza>? Pizzas { get; set; } // Collection Navigation property
    }

    public class PizzaContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }

        private string DatabaseFileName { get; }

        public PizzaContext() : this("pizzas.db")
        {

        }

        public PizzaContext(string databaseFileName)
        {
            DatabaseFileName = databaseFileName;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            string folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, DatabaseFileName);

            SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
            {
                DataSource = filePath
            };

            optionsBuilder
                .UseSqlite(csb.ConnectionString)
                .LogTo(log => Debug.WriteLine(log))
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Pizza>()
                .HasMany(p => p.Incrediences)
                .WithMany(i => i.Pizzas)
                .UsingEntity<PizzaIncredience>();

            modelBuilder.Entity<PizzaStyle>().HasData(
                new PizzaStyle() { Id = 1, Description = "Italsky styl" },
                new PizzaStyle() { Id = 2, Description = "Americky styl" });

            modelBuilder.Entity<Pizza>().HasData(
                new Pizza() { Id = 1, Name = "Margherita", Price = 100, PizzaStyleId = 1 },
                new Pizza() { Id = 2, Name = "Salami", Price = 130, PizzaStyleId = 1 },
                new Pizza() { Id = 3, Name = "Funghi", Price = 135, PizzaStyleId = 2 }
            );

            modelBuilder.Entity<Incredience>().HasData(
                new Incredience() { Id = 1, Name = "Cibule" },
                new Incredience() { Id = 2, Name = "Hranolky" });

            modelBuilder.Entity<PizzaIncredience>().HasData(
                new PizzaIncredience() { PizzaId = 1, IncredienceId = 1 },
                new PizzaIncredience() { PizzaId = 1, IncredienceId = 2 });
        }

    }

}