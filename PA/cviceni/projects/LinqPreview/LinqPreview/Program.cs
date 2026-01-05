using System.Text;
using LinqPreview;
using TinyCsvParser;
using TinyCsvParser.Enums;
using TinyCsvParser.TypeConverter;

#region init
var csvParserOptions = new CsvParserOptions(true, ',');
var typeConverter = new TypeConverterProvider().AddEnums();
var csvMapper = new Mapper(typeConverter);
var csvParser = new CsvParser<Order>(csvParserOptions, csvMapper);

var orderList =  csvParser.ReadFromFile(@"eshop-order-database.csv", Encoding.ASCII).Select(x=>x.Result).ToList();
#endregion

SpectreTableGenerator.RenderTable(orderList, true);

// To keep the console open while using External Console
Console.ReadKey();