using System.ComponentModel;

namespace Utb.Studenti.Models
{
    public class Student
    {
        public int Id { get; set; }
        public required string Jmeno { get; set; }
        public required int SkupinaId { get; set; } // Cizi klic
        public Skupina? Skupina { get; set; } // Navigation property, pouze pro praci s objekty v pameti

        public ICollection<Predmet> Predmety { get; set;} = new List<Predmet>();
    }



}