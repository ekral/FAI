namespace WebAPI.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public required int ProductId { get; set; }
        public required int TransactionTypeId { get; set; }
        public required int QuantityChanged { get; set; }
        public required DateTime TransactionDate { get; set; }
        public Product? Product { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}