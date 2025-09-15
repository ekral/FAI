using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Npgsql;
using Pgvector;
using System.Runtime.CompilerServices;

namespace ConsoleAppRag
{
    class Recept
    {
        public int Id { get; set; }
        public required string Obsah { get; set; }
        public required Vector Embeddings { get; set; }
    }

    class MyDbContext : DbContext
    {
        public DbSet<Recept> Recepty { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            NpgsqlConnectionStringBuilder sb = new()
            {
                Host = "localhost",
                Port = 5432,
                Database = "mydb",
                Username = "postgres",
                Password = "admin"
            };

            optionsBuilder.UseNpgsql(sb.ConnectionString, o => o.UseVector());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");
        }
    }

    internal class Program
    {
        static async Task CreateDb()
        {
            using MyDbContext context = new();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        static async Task SeedAsync()
        {
            using MyDbContext context = new();

            var client = new OpenAI.OpenAIClient("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");

            var embeddingGenerator = client.GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator();

            string text1 = "Recept na vyrobu kolacku: mouka vejce voda cukr, vse smichame, nechame vykynout a pak upeceme";
            ReadOnlyMemory<float> embedding1 = await embeddingGenerator.GenerateVectorAsync(text1);

            string text2 = "Recept na koktej: vodka dzus, promichame vodku s dzusem a pridame led";
            ReadOnlyMemory<float> embedding2 = await embeddingGenerator.GenerateVectorAsync(text2);

            string text3 = "Recept na puding z prasku: prasek, 1/2 l mleka, prasek rozmichame v 1/3 mleka, zbyle mleko privedeme k varu a pak prilijeme mleko s rozpustenym praskem a varime jeste 2 minuty na mirnem plameni.";
            ReadOnlyMemory<float> embedding3 = await embeddingGenerator.GenerateVectorAsync(text3);

            Recept[] recepty = [

                new Recept() { Obsah = text1, Embeddings = new Vector(embedding1)},
                new Recept() { Obsah = text2, Embeddings = new Vector(embedding2)},
                new Recept() { Obsah = text3, Embeddings = new Vector(embedding3)},

                ];

            context.Recepty.AddRange(recepty);

            int count = await context.SaveChangesAsync();
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("1 Create db");
            Console.WriteLine("2 Seed data");
            Console.WriteLine("0 exit");

            string choice;

            do
            {
                choice = Console.ReadLine() ?? "0";

                switch (choice)
                {
                    case "1":
                        await CreateDb();
                        break;
                    case "2":
                        await SeedAsync();
                        break;
                }

            } while (choice != "0");
        }
    }
}
