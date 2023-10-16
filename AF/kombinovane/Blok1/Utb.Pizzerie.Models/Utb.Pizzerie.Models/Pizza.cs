using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Utb.Pizzerie.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public required string Nazev { get; set; }
    }

    public class PizzaContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }

        private string dbPath = "pizza.db";

        public PizzaContext()
        {

        }

        public PizzaContext(string dbPath)
        {
            this.dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            string folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, dbPath);

            SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
            {
                DataSource = filePath
            };
            optionsBuilder.UseSqlite(csb.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Pizza>()
                .HasData(new Pizza() { Id = 1, Nazev = "Prvni" }, new Pizza() { Id = 2, Nazev = "Druha" });
        }
    }
}