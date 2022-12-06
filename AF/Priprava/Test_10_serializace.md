Dokumentace: [Serialization in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)

## Serialization

Objekty v paměti nejsou kompatibilní mezi hardwarovými platformami (AMR, x86, x64) a už vůbec mezi různými programovacími jazyky. Serializace je proces převodu objektu v paměti na jiný formát kompatibilní mezi různými platformami. Například text ve formátu JSON, který můžeme uložit do souboru nebo přenést po síti. 

Máme několik možností:

- člověkem čitelný textový formát, nejběžnější je JSON používaný pro REST webové služby založené na HTTP protokolu. Dříve se používal XML formát pro SOAP služby.
- binární formát pro vysoký výkon. Příkladem je protokol gRPC.

Dotnet obsahuje mimo jiné zabudovanou podporu pro serializaci a deserializaci do formátu JSON. Není nutné vkládat žádný nuget balíček.

Máme následující třídu:

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

Následují příklad serializuje studenta.


```csharp
Student student = new Student() { Id = 1, Name = "Mikulas" };
string text = JsonSerializer.Serialize(student);
```

Výsledný řetězec v proměnné text bude:
```json
{
    "Id" : 1,
    "Name" : "Mikulas"
}
```

A následující příklad zase z řetězce deserializuje studenta.

```csharp
string text = "{ \"Id\" = 1, \"Name\" = \"Cert\"  }";
Student? student = JsonSerializer.Deserialize<Student>(text);
```

V C#11 z .NET 7 můžeme použít raw string literals a zadávat dvojité uvozovky přímo v textu řetězce bez escape sekvence.
 
 ```csharp
string text = """{ "Id" = 1, "Name" = "Cert"  }""";
Student? student = JsonSerializer.Deserialize<Student>(text);
```
