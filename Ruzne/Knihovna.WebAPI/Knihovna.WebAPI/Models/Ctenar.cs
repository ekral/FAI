namespace Knihovna.WebAPI.Models
{
    public class Ctenar
    {
        public int CtenarId { get; set; }
        public required string Jmeno { get; set; }
        public List<Vypujcka>? Vypujcky { get; set; }
    }
}
