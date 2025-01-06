namespace InventoryTrackingSystem.Models
{
    // Product entity representing items sold or stored in inventory
    public class Product
    {
        public int ProductId { get; set; }  // Primary Key
        required public string Name { get; set; }    // Product name
        required public string Description { get; set; }  // Detailed description
        required public decimal Price { get; set; }  // Product price
        required public int CategoryId { get; set; }  // Foreign Key to Category
        public Category? Category { get; set; }  // Navigation property to Category
        public ICollection<InventoryItem>? InventoryItems { get; set; }  // One-to-many relationship
        public ICollection<Transaction>? Transactions { get; set; }  // One-to-many relationship
    }

    // Category entity for categorizing products (e.g., Electronics, Furniture)
    public class Category
    {
        public int CategoryId { get; set; }  // Primary Key
        required public string Name { get; set; }  // Name of the category
        public ICollection<Product>? Products { get; set; }  // One-to-many relationship
    }

    // InventoryItem represents each unit of a product in inventory
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }  // Primary Key
        required public int ProductId { get; set; }  // Foreign Key to Product
        required public Product Product { get; set; }  // Navigation property to Product
        required public int Quantity { get; set; }  // Quantity in stock
        required public DateTime DateAdded { get; set; }  // Date item was added to inventory
    }

    // Transaction entity to track stock movements (e.g., stock addition or sale)
    public class Transaction
    {
        public int TransactionId { get; set; }  // Primary Key
        required public int ProductId { get; set; }  // Foreign Key to Product
        public Product? Product { get; set; }  // Navigation property to Product
        required public int QuantityChanged { get; set; }  // Quantity added or removed
        required public DateTime TransactionDate { get; set; }  // Date of the transaction
        required public int TransactionTypeId { get; set; }  // Foreign Key to TransactionType
        public InventoryTransactionType? TransactionType { get; set; }  // Navigation property to TransactionType
    }

    // InventoryTransactionType to classify types of transactions (e.g., Sale, Add, Return)
    public class InventoryTransactionType
    {
        public int TransactionTypeId { get; set; }  // Primary Key
        required public string Type { get; set; }  // Transaction type name (e.g., Sale, Add, Return)
        public ICollection<Transaction>? Transactions { get; set; }  // One-to-many relationship
    }

    // Supplier entity to represent suppliers providing the products
    public class Supplier
    {
        public int SupplierId { get; set; }  // Primary Key
        required public string Name { get; set; }  // Name of the supplier
        required public string ContactEmail { get; set; }  // Contact email for the supplier
        required public string ContactPhone { get; set; }  // Contact phone for the supplier
        public ICollection<Product>? Products { get; set; }  // One-to-many relationship with Product
    }
}
