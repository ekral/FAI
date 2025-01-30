using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEF
{
    public class Student
    {
        public int StudentId { get; set; } // Primární klíč dle jmenných konvencí
        public required string Jmeno { get; set; }
        public required string Prijmeni { get; set; }
        public int SkupinaId { get; set; }
        public Skupina? Skupina { get; set; } // Navigation Property
    }

    public class Skupina
    {
        public int SkupinaId { get; set; }
        public required string Nazev { get; set; } 
        public ICollection<Student>? Studenti { get; set; } // Navigation Property
    }
}
