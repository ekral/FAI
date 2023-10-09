using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class PizzaKioskContext : DbContext
    {
        public DbSet<PizzaConfigurationOption> PizzaOptions { get; set; } 

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
            var option1 = new StringOptions()
            {
                PizzaConfigurationOptionId = 1,
                Description = "Pizza size",
                Options = new string[] { "Small", "Medium", "Large" },
                DefaultValueIndex = 1
            };

            var option2 = new BooleanOption()
            {
                PizzaConfigurationOptionId = 2,
                Description = "Garling",
                DefaultValue = true
            };

            var option3 = new QuantityOption()
            {
                PizzaConfigurationOptionId = 3,
                Description = "Number of pfeferoni",
                MinimalValue = 0,
                MaximalValue = 10,
                DefaultValue = 1
            };

            var value1 = new StringValue()
            {
                Id = 1,
                StringOptionsId = 1,
                SelectedString = "Small"
            };

            var value2 = new BooleanValue()
            {
                Id = 2,
                BooleanOptionId = 2,
                IsSelected = true,
            };

            var value3 = new QuantityValue()
            {
                Id = 3,
                QuantityOptionId = 3,
                Quantity = 5,
            };

            modelBuilder.Entity<PizzaConfigurationOption>().HasKey(so => so.PizzaConfigurationOptionId);
            modelBuilder.Entity<StringOptions>().HasData(option1);
            modelBuilder.Entity<BooleanOption>().HasData(option2);
            modelBuilder.Entity<QuantityOption>().HasData(option3);
            modelBuilder.Entity<StringOptions>()
                .Property(x => x.Options)
                .HasConversion(
                    options => JsonSerializer.Serialize(options, (JsonSerializerOptions)null), 
                    jsonString => JsonSerializer.Deserialize<ICollection<string>>(jsonString, (JsonSerializerOptions)null));
        }
    }
}
