# 04 Unit testy Minimal Web API

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu si probereme [testování Web API s produkční databází](https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database) a frameworkem [xUnit](https://xunit.net/).

Nejprve si nadefinujeme entity a DbContext:

```csharp
public class Kniha
{
    public int KnihaId { get; set; }
    public required string Nazev { get; set; }
    public Vypujcka? Vypujcka { get; set; }
}

public class KnihovnaContext(DbContextOptions<KnihovnaContext> options) : DbContext(options)
{
    public DbSet<Ctenar> Ctenari { get; set; }
    public DbSet<Kniha> Knihy { get; set; }
    public DbSet<Vypujcka> Vypujcky { get; set; }
}
```

Dále si do projektu přidáme nový projekt s názvem "xUnit Test Project" a v tomto novém projektu přidáme referenci na projekt obsahující entity a DbContext. 

Nástroj xUnit organizuje testovací metody do tříd, kdy testovací metody ve třídě se spouští sekvenčně a testy v různých třídách se potom pouští souběžně. My budeme pracovat s produkční databází, tedy s typem databáze kterou budeme používat v produkci. Aby se testy vzájemně neovlivńovali, tak by si teoreticky mohla každá testovací metoda vytvořit vlastní databázi. Prakticky by to ale výrazně zpomalilo provádění testů. Proto si vytvoříme jen jednu databázi. 