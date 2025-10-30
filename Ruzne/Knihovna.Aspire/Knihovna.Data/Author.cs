using System.Collections.Generic;

namespace Knihovna.Data
{

    public class Autor
    {
        public int Id { get; set; }
        public required string Jmeno { get; set; }
        public required string Prijmeni { get; set; }
        public List<Kniha> Knihy { get; } = [];
    }
}
