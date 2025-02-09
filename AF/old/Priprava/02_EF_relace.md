# Relace v Entity Frameworku

[Introduction to relationships](https://learn.microsoft.com/en-us/ef/core/modeling/relationships)

```csharp
public class Student
{
    public int Id { get; set; }
    public required string Jmeno { get; set; }
    public int SkupinaId { get; set; } // Cizi klic
    public Skupina? Skupina { get; set; } // Navigation property, pouze pro praci s objekty v pameti
}
```

```csharp
public class Skupina
{
    public int Id { get; set; }
    public required string Nazev { get; set; }

    public ICollection<Student>? Studenti { get; set; } // Navigation property, jen pro program ne pro db
}
```
