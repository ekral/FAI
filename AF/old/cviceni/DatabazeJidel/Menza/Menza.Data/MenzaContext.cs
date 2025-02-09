using Menza.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Menza.Data
{
    public class MenzaContext : DbContext
    {
        public DbSet<Jidlo> Jidla { get; set; }
        

        public MenzaContext()
        {

        }

        public MenzaContext(DbContextOptions<MenzaContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var folder = Environment.SpecialFolder.MyDocuments;
            var folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, "Menza.db");

            SqliteConnectionStringBuilder stringBuilder = new()
            {
                DataSource = filePath
            };

            optionsBuilder.UseSqlite(stringBuilder.ConnectionString);
        }
    }
}