using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Prednaska.Studenti
{
    public class Student
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }
        public int SkupinaId { get; set; }
        public Skupina Skupina { get; set; } // pouze navigation property

        public ICollection<Predmet> Predmety { get; set; }

        public List<Poznamka> Poznamky { get; set; }

    }

    class Poznamka
    {
        public int Id { get; set; }
        public string Poznamka { get; set; }
    }

    public class Predmet
    {
        public int Id { get; set; }
        public required string Nazev { get; set; }

        public ICollection<Student> Studenti { get; set; }

    }

    class StudentNaPredmetu
    {
        public int StudentId { get; set; }
        public int PredmetId { get; set; }
    }

    public class Skupina
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
    }

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
            modelBuilder.Entity<Student>().HasMany(s => s.Predmety).WithMany(p => p.Studenti).UsingEntity<StudentNaPredmetu>();

            Skupina skupina = new Skupina() { Id = 1, Nazev = "swi" };
            Student student = new Student() { Id = 1, Jmeno = "Bohumil", SkupinaId = 1 };
            Predmet predmet = new Predmet() { Id = 1, Nazev = "Telocvik" };

            StudentNaPredmetu studentNaPredmetu = new StudentNaPredmetu() { StudentId = 1, PredmetId = 1 };

            modelBuilder.Entity<Skupina>().HasData(skupina);
            modelBuilder.Entity<Student>().HasData(student);
            modelBuilder.Entity<Predmet>().HasData(predmet);
            modelBuilder.Entity<StudentNaPredmetu>().HasData(studentNaPredmetu);
        }
    }
}