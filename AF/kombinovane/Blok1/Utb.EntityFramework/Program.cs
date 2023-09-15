using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Utb
{
    class Student
    {
        public int Id { get; set; }
        public required string Jmeno { get; set; }
        public required double Body { get; set; }
    }

    class SkolaContext : DbContext
    {
        DbSet<Student> Studenti { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.MyDocuments;
            string folderPath = Environment.GetFolderPath(folder);
            string filePath = System.IO.Path.Join(folderPath, "database.db");

            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder
            {
                DataSource = filePath
            };

            optionsBuilder.UseSqlite(builder.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Student> studenti = new List<Student>()
            {
                new Student() { Jmeno = "Jolana", Body = 100.0},
                new Student() { Jmeno = "Karel", Body = 7.0},
                new Student() { Jmeno = "Pavel", Body = 55.0}
            };

            modelBuilder.Entity<Student>().HasData(studenti);
        }
    }
    

    internal class Program
    {
        static void Main(string[] args)
        {

            List<Student> studenti = new List<Student>()
            {
                new Student() { Jmeno = "Jolana", Body = 100.0},
                new Student() { Jmeno = "Karel", Body = 7.0},
                new Student() { Jmeno = "Pavel", Body = 55.0}
            };

            // chci ziskat kolekci studenti, kteri splnili zapocet z predmetu AP3AF
            IEnumerable<Student> uspesniStudenti = studenti.Where(s => s.Body >= 50);

            foreach(Student student in uspesniStudenti)
            {
                Console.WriteLine($"jmeno: {student.Jmeno} body: {student.Body}");
            }

            
        }
    }
}