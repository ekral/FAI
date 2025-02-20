using System.Collections.Generic;

namespace Studenti.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public required bool Studuje { get; set; }
    }

}
