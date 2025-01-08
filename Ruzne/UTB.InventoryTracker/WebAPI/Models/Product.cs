namespace WebAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required decimal UnitPrice { get; set; }

        public required int QuantityOnHand { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}