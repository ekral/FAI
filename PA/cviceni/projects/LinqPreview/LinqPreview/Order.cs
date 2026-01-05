using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace LinqPreview;

public record Order
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public DateTime Date { get; init; }
    public decimal Price { get; init; }
    public int TotalItems { get; init; }
    public string ItemList { get; init; } = "";
    public bool IsExpressShipping { get; init; }
    public CustomerTier Tier { get; init; }
    public decimal DiscountApplied { get; init; }
}

public enum CustomerTier
{
    Bronze,
    Silver,
    Gold,
    Platinum
}

public class Mapper : CsvMapping<Order>
{
    public Mapper(ITypeConverterProvider provider) : base(provider)
    {
        MapProperty(0, x => x.Id);
        MapProperty(1, x => x.Name);
        MapProperty(2, x => x.Date);
        MapProperty(3, x => x.Price);
        MapProperty(4, x => x.TotalItems);
        MapProperty(5, x => x.ItemList);
        MapProperty(6, x => x.IsExpressShipping);
        MapProperty(7, x => x.Tier);
        MapProperty(8, x => x.DiscountApplied);
    }
}