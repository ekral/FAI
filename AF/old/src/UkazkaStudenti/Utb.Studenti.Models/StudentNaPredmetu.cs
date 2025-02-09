using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.Studenti.Models
{
    public class StudentNaPredmetu
    {
        public required int StudentId { get; set; }
        public required int PredmetId { get; set; }
    }
}
