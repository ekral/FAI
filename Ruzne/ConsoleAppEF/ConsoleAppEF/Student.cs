using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEF
{
    public class Student
    {
        public int Id { get; set; } // Primární klíč dle jmenných konvencí
        public required string Jmeno { get; set; }
        public required string Prijmeni { get; set; }
    }

}
