namespace Knihovna.Data
{
    public class Kniha
    {
        public int Id { get; set; }
        public int AutorId { get; set; }
        public required string Nazev { get; set; }

        public Autor? Autor { get; set; }
    }
}
