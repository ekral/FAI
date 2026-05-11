using System.ComponentModel.DataAnnotations;

namespace UTB.ConcurrencyTest.WebApi
{
    class Menu
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required int Quantity { get; set; }

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
