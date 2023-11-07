# Cvičení: Serializace a deserializace Studenta

Máte třídu Student:
```csharp
class Student
{
    public int Id { get; set; }     
    public required string Jmeno { get; set; }
}
```

S využitím [JsonSerializer](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) z namespace `System.Text.Json`:

1) Vytvořte kód, který serializuje instanci třídy Student (objekt v paměti) do řetězce ve formátu JSON a vypište řezetec na konzoli.
2) Zadejte v kódu JSON řetězec reprezentující studenta s využitím [Raw string literals](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#raw-string-literals) a deserializujte řetězec na instanci třídy Student (objekt v paměti).
