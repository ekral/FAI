# 02 Relace v entity frameworku

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu probereme relace one to many (1 : n) a many to many (n : m).

## Relace One to Many

V následujícím příkladu budeme předpokládat, že student může být zapsaný jen v jedné studijní skupině. Student obsahuje cizí klíč `SkupinaId` a navigační property Skupina. Navigační property slouží čistě pro procházení dat a nepoužívá se pro přidání nového řádku nebo aktualizaci řádku. Pro přidání řádku nebo aktualizaci se používá cizí klíč.

Třída `Skupina` potom obsahuje jen navigační property `Studenti`. Tato properta opět slouží pro procházení dat.


```csharp
public class Student
{
    public int StudentId { get; set; } // Primární klíč dle jmenných konvencí
    public required string Jmeno { get; set; }
    public required string Prijmeni { get; set; }
    public int SkupinaId { get; set; }
    public Skupina? Skupina { get; set; } // Navigation Property
}

public class Skupina
{
    public int SkupinaId { get; set; }
    public required string Nazev { get; set; } 
    public ICollection<Student>? Studenti { get; set; } // Navigation Property
}
```

Dále si vytvoříme DbContext:
```csharp
public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
{
    public DbSet<Student> Studenti { get; set; }
    public DbSet<Skupina> Skupiny { get; set; }
}
```

Nyní si projdeme vytváření a načítání dat.

### Nový řádek databáze





[Create Model](https://learn.microsoft.com/en-us/ef/core/modeling/)