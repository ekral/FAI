namespace Knihovna.WebAPI.Models
{
    public class Vypujcka
    {
        public int VypujckaId { get; set; }
        public required int KnihaId { get; set; }
        public required int CtenarId { get; set; }   
        public required DateOnly DatumVypujcky { get; set; }
        public DateOnly? DatumVraceni { get; set; }
        public Kniha? Kniha { get; set; }
        public Ctenar? Ctenar { get; set; }
    }
}
