// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Utb.PizzaKiosk.Models;

string json = """
{
      "Id": 0,
      "Name": "Onion",
      "QuantityDescription": "15 g",
      "UnitPrice": 35,
      "AlergensList": "[1,2,3]"
}
""";

Ingredient? ingredient = JsonSerializer.Deserialize<Ingredient>(json);

if(ingredient is not null)
{
    Console.WriteLine(ingredient.Name);
}

Console.WriteLine(JsonSerializer.Serialize(ingredient));