namespace Knihovna.WebAPI.Models
{
    public class Vypujcka
    {
        public int VypujckaId { get; set; }
        public int KnihaId { get; set; }
        public int CtenarId { get; set; }
        public DateOnly DatumVypujcky { get; set; }
        public DateOnly? DatumVraceni { get; set; }
    }
}
