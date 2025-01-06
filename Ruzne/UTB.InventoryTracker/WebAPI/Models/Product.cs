namespace WebAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        required public string Name { get; set; }
        required public string Description { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        required public string Name { get; set; }
        required public string Description { get; set; }
        required public int CategoryId { get; set; }
        public Category? Category { get; set; }
        required public decimal Price { get; set; }
        required public string StockKeepingUnit { get; set; }
    }


}
