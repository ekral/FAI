using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OllamaSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;

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

            modelBuilder.Entity<Recept>().HasIndex(i => i.Embeddings)
            .HasMethod("hnsw")
            .HasOperators("vector_cosine_ops")
            .HasStorageParameter("m", 16)
            .HasStorageParameter("ef_construction", 64);

            // velikost kterou vraci openAI embedding model
            modelBuilder.Entity<Recept>().Property(p => p.Embeddings).HasColumnType("vector(1536)"); // OpenAI 1536, Gemma 768
        }
    }

    internal class Program
    {
        static async Task CreateDbAsync()
        {
            using MyDbContext context = new();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        static async Task SeedAsync(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            using MyDbContext context = new();

            string text1 = "Recept na vyrobu koláčku: mouka vejce voda cukr, vše smícháme, necháme vykynout a pak upečeme";
            ReadOnlyMemory<float> embedding1 = await embeddingGenerator.GenerateVectorAsync(text1);

            string text2 = "Recept na koktej: vodka džus, promíchíme vodku s džusem a přidáme led";
            ReadOnlyMemory<float> embedding2 = await embeddingGenerator.GenerateVectorAsync(text2);

            string text3 = "Recept na puding z prášku: prášek, 1/2 l mléka, prášek rozmícháme v 1/3 mléka, zbylé mléko přivedeme k varu a pak přilijeme mléko s rozpuštěnám práškem a vaříme ješte 2 minuty na mírném plameni.";
            ReadOnlyMemory<float> embedding3 = await embeddingGenerator.GenerateVectorAsync(text3);

            Recept[] recepty = [

                new Recept() { Obsah = text1, Embeddings = new Vector(embedding1)},
                new Recept() { Obsah = text2, Embeddings = new Vector(embedding2)},
                new Recept() { Obsah = text3, Embeddings = new Vector(embedding3)},

                ];

            context.Recepty.AddRange(recepty);

            int count = await context.SaveChangesAsync();
        }
        
        static async Task TestQueryAsync(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            using MyDbContext context = new();

            Console.WriteLine("Zadejte dotaz:");
            string question = Console.ReadLine() ?? "";

            ReadOnlyMemory<float> embedding = await embeddingGenerator.GenerateVectorAsync(question);
            Vector vector = new(embedding);

            var results = await context.Recepty.Select(o => new { Distance = o.Embeddings.CosineDistance(vector), Recept = o }).OrderBy(r => r.Distance).ToListAsync();

            foreach (var result in results)
            {
                Console.WriteLine($"Distance: {result.Distance} Nejlepsi odpoved: {result.Recept.Obsah}");
            }
        }

        static async Task<string[]> DatabazeReceptu(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, string question)
        {
            Console.WriteLine($"Tool byl zavolany: {question}");

            using MyDbContext context = new();

            ReadOnlyMemory<float> embedding = await embeddingGenerator.GenerateVectorAsync(question);
            Vector vector = new(embedding);

            string[] results = await context.Recepty.OrderBy(r => r.Embeddings.CosineDistance(vector)).Take(2).Select(r => r.Obsah).ToArrayAsync();

            return results;
        }

        static async Task TestChatAsync(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, IChatClient chatClient)
        {
            Console.WriteLine("Zadejte dotaz:");
            string question = Console.ReadLine() ?? string.Empty;

            string systemPrompt = "Jsi asistent pro vyhledavani v databazi receptu. Na zaklade dotazu uzivatele vyhledej nejvhodnejsi recepty a odpovez na dotaz uzivatele. Pokud dotaz nesouvisi s recepty, odpovez ze se muze ptat jen na recepty. Recepty jsou v ceskem jazyce. Recepty si nevymyslej, vzdy volej tool a pouzij vraceny popis receptu z tohoto toolu.";
            string toolDescription = "Funkce pro vyhledani v databazi receptu. Vstupem je dotaz uzivatele, vystupem jsou nejvhodnejsi recepty. Pouzij pouze pokud se uzivatel ptal na nejaky recept.";

            ChatResponse chatResponse = await chatClient.GetResponseAsync([new ChatMessage(ChatRole.System, systemPrompt), new ChatMessage(ChatRole.User, question)], new ChatOptions() { Tools = [AIFunctionFactory.Create(async (string question) => await DatabazeReceptu(embeddingGenerator, question), description: toolDescription)] });
            
            Console.WriteLine(chatResponse.Text);
        }

        static async Task Main(string[] args)
        {
            ConfigurationBuilder builder = new();
            builder.AddUserSecrets("b6aaee32-2170-4564-b3d9-1926f7dd3188");
            IConfigurationRoot configuration = builder.Build();
            string key = configuration["ApiKey"] ?? throw new InvalidOperationException();

            string choice;


            Console.WriteLine("1 OpenAIClient");
            Console.WriteLine("2 OllamaClient");

            choice = Console.ReadLine() ?? string.Empty;

            (IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, IChatClient chatClient) = choice switch
            {
                "1" => new OpenAI.OpenAIClient(key) is OpenAI.OpenAIClient cli ? (cli.GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator(), cli.GetChatClient("gpt-5-mini").AsIChatClient().AsBuilder().UseFunctionInvocation().Build()) : throw new InvalidOperationException(),
                "2" => new OllamaApiClient("http://localhost:11434", "embeddinggemma") is OllamaApiClient cli ? (cli, new ChatClientBuilder(cli).UseFunctionInvocation().Build()) : throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };

     
            do
            {
                Console.WriteLine("1 Create db");
                Console.WriteLine("2 Seed data");
                Console.WriteLine("3 Dotazovani");
                Console.WriteLine("4 Chat");
                Console.WriteLine("0 exit");

                choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        await CreateDbAsync();
                        break;
                    case "2":
                        await SeedAsync(embeddingGenerator);
                        break;
                    case "3":
                        await TestQueryAsync(embeddingGenerator);
                        break;
                    case "4":
                        await TestChatAsync(embeddingGenerator, chatClient);
                        break;
                }

            } while (choice != "0");
        }
    }
}
