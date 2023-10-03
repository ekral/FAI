using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Predmety)
                .WithMany(p => p.Studenti)
                .UsingEntity<StudentNaPredmetu>();

            Predmet predmet = new Predmet() { Id = 1, Nazev = "Telocvik" };
            Skupina skupina = new Skupina() { Id = 1, Nazev = "swi1" };
            Student student = new Student() { Id = 1, SkupinaId = 1, Jmeno = "Bohumil" };
            StudentNaPredmetu studentNaPredmetu = new StudentNaPredmetu() { PredmetId = 1, StudentId = 1 };

            
            modelBuilder.Entity<Predmet>().HasData(predmet);
            modelBuilder.Entity<Skupina>().HasData(skupina);
            modelBuilder.Entity<Student>().HasData(student);
            modelBuilder.Entity<StudentNaPredmetu>().HasData(studentNaPredmetu);


        }
    }
}
