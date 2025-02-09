using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.Studenti.Models
{
    public class Predmet
    {
        public int Id { get; set; }
        public required string Nazev { get; set; }
        public ICollection<Student> Studenti { get; set; } = new List<Student>();
    }
}
