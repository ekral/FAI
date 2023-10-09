using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Utb.Studenti.Models
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Skupina> Skupiny { get; set; }

        private string dbPath = "studenti.db";

        public StudentContext()
        {

        }

        public StudentContext(string dbPath)
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
                .Entity<Student>()
                .HasMany(s => s.Predmety)
                .WithMany(p => p.Studenti)
                .UsingEntity<StudentNaPredmetu>();

            modelBuilder
                .Entity<Predmet>()
                .HasData(
                    new Predmet() { Id = 1, Nazev = "Telocvik" });

            modelBuilder
                .Entity<Skupina>()
                .HasData(
                    new Skupina() { Id = 1, Nazev = "swi1" });

            modelBuilder
                .Entity<Student>()
                .HasData(
                    new Student() { Id = 1, SkupinaId = 1, Jmeno = "Bohumil" }, 
                    new Student() { Id = 2, SkupinaId = 1, Jmeno = "Stefan" });

            modelBuilder
                .Entity<StudentNaPredmetu>()
                .HasData(
                    new StudentNaPredmetu() { PredmetId = 1, StudentId = 1 }, 
                    new StudentNaPredmetu() { PredmetId = 1, StudentId = 2 });
        }
    }
}
