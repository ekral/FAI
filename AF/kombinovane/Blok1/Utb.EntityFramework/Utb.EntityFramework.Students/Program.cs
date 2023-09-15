using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;

namespace Utb
{
    class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required double Points { get; set; }
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
                new Student() { Name = "Jolana", Points = 100.0},
                new Student() { Name = "Karel", Points = 7.0},
                new Student() { Name = "Pavel", Points = 55.0}
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
                new Student() { Name = "Jolana", Points = 100.0},
                new Student() { Name = "Karel", Points = 7.0},
                new Student() { Name = "Pavel", Points = 55.0}
            };

            // chci ziskat kolekci studenti, kteri splnili zapocet z predmetu AP3AF
            IEnumerable<Student> uspesniStudenti = studenti.Where(s => s.Points >= 50);

            foreach (Student student in uspesniStudenti)
            {
                Console.WriteLine($"jmeno: {student.Name} body: {student.Points}");
            }
        }
    }
}