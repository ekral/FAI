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
        public DbSet<Student> Studenti { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.MyDocuments;
            string folderPath = Environment.GetFolderPath(folder);
            string filePath = System.IO.Path.Join(folderPath, "database.db");

            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder
            {
                DataSource = filePath
            };

            optionsBuilder
                .UseSqlite(builder.ConnectionString)
                .LogTo(log => System.Diagnostics.Debug.WriteLine(log));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Student> studenti = new List<Student>()
            {
                new Student() { Id = 1, Name = "Jolana", Points = 100.0},
                new Student() { Id = 2, Name = "Karel", Points = 7.0},
                new Student() { Id = 3, Name = "Pavel", Points = 55.0}
            };

            modelBuilder.Entity<Student>().HasData(studenti);
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            using SkolaContext skolaKontext = new SkolaContext();

            IQueryable<Student> uspesniStudenti = skolaKontext.Studenti.Where(s => s.Points >= 50.0);

            foreach (Student student in uspesniStudenti)
            {
                Console.WriteLine($"jmeno: {student.Name} body: {student.Points}");
            }

            //Student novy = new Student() { Name = "Alice", Points = 80.0 };

            //skolaKontext.Studenti.Add(novy);

            //int pocet = await skolaKontext.SaveChangesAsync();

            //Console.WriteLine(novy.Id);

        }
    }
}