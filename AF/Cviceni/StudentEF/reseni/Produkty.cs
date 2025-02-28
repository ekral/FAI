using Microsoft.EntityFrameworkCore;

namespace ConsoleAppProdukty2
{

    class Product
    {
        public int ProductId { get; set; }
        public required string Nazev { get; set; }
        public required decimal Cena { get; set; }
    }

    class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }


    internal class Program
    {

        public static ProductContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseSqlite("DataSource=products.db")
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging()
                .Options;

            return new ProductContext(options);
        }

        public static async Task Seed()
        {
            await using ProductContext context = CreateContext();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Products.AddRangeAsync(
                new Product() { Nazev = "Telefon", Cena = 10.0m },
                new Product() { Nazev = "Notebook", Cena = 25.0m },
                new Product() { Nazev = "Monitor", Cena = 17.3m }
                );

            await context.SaveChangesAsync();
        }

        public static async Task<List<Product>> GetAllProducts()
        {
            await using ProductContext context = CreateContext();

            return await context.Products.OrderBy(s => s.Nazev).ToListAsync();
        }

        public static async Task<decimal> GetAveragePrice()
        {
            await using ProductContext context = CreateContext();

            return await context.Products.AverageAsync(s => s.Cena);
        }

        static async Task Main(string[] args)
        {
            await Seed();

            List<Product> produkty = await GetAllProducts();

            foreach (Product product in produkty)
            {
                Console.WriteLine($"{product.Nazev} {product.Cena}");
            }

            decimal averagePrice = await GetAveragePrice();

            Console.WriteLine($"Average price: {averagePrice}");
        }
    }
}
