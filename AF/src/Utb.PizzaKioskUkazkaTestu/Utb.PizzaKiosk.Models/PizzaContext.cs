using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Utb.PizzaKiosk.Models
{
    public class PizzaContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }

        private string dbPath = "pizza_2.db";

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
            modelBuilder.Entity<Pizza>().HasData(
                new Pizza() { Id = 1, Jmeno = "Hawai", Description="s ananasem", Price=120},
                new Pizza() { Id = 2, Jmeno = "Syrova", Description = "se syrem", Price = 130 },
                new Pizza() { Id = 3, Jmeno = "Sunkova", Description = "se sunkou", Price = 140 }
            );
        }
    }


}