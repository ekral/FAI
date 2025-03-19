namespace Knihovna.WebAPI.Models
{
    public class Kniha
    {
        public int KnihaId { get; set; }
        public required string Nazev { get; set; }
        public List<Vypujcka> Vypujcky { get; set; } = [];
    }
}
