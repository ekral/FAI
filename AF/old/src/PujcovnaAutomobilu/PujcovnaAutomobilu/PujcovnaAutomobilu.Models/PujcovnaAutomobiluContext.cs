using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace PujcovnaAutomobilu.Models
{
    public class PujcovnaAutomobiluContext : DbContext
    {
        public DbSet<Automobil> Automobils { get; set; }

        public PujcovnaAutomobiluContext()
        {

        }

        public PujcovnaAutomobiluContext(DbContextOptions<PujcovnaAutomobiluContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured)
            {
                return;
            }

            Environment.SpecialFolder folder = Environment.SpecialFolder.MyDocuments;
            string folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, "PujcovnaAutomobilu.db");

            SqliteConnectionStringBuilder builder = new()
            {
                DataSource = filePath
            };

            optionsBuilder.UseSqlite(builder.ConnectionString);
        }


    }
}
