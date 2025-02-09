namespace PujcovnaAutomobilu.Models
{
    public class Automobil
    {
        public int Id { get; set; }
        public required string Model { get; set; }
        public required bool Pujceno { get; set; }
    }
}