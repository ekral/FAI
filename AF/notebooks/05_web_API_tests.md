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

Nástroj xUnit organizuje testovací metody do tříd, kdy testovací metody ve třídě se spouští sekvenčně a testy v různých třídách se potom pouští souběžně. My budeme pracovat s produkční databází, tedy s typem databáze kterou budeme používat v produkci. Aby se testy vzájemně neovlivňovaly, tak by si teoreticky mohla každá testovací metoda vytvořit vlastní databázi. Prakticky by to ale výrazně zpomalilo provádění testů. Proto si vytvoříme jen jednu databázi, kterou budeme sdílet mezi všemi testovacími metodami. 

xUnit vytváří instanci testovací třídy pro každou metodu. Proto musíme použít [Class Fixture](https://xunit.net/docs/shared-context#class-fixture), což je způsob jak sdílet jednu instanci databáze mezi testovacími metodami v jedné třídě. Mohli bychom také použít [Collection Fixtures](https://xunit.net/docs/shared-context#class-fixture), ale to by zabránilo souběžnému provádění testovacích metod v různých třídách a zpomalilo by spouštění testů. Proto používáme kritickou sekci a sdílíme instanci DbContextu mezi více testovacími třídami. Pokud by se testy v třídách vzájemně ovlivňovaly, tak můžeme použít více databází a vytvořit více Class Fixtur pro různé třídy.

Následující kód představuje Class Fixture:

```csharp
public class DatabaseFixture
{
    private static readonly Lock _lock = new();
    private static bool _databaseInitialized = false;

    public DatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using KnihovnaContext context = CreateContext();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Knihy.AddRange(
                    new Kniha() { KnihaId = 1, Nazev = "Svejk" },
                    new Kniha() { KnihaId = 2, Nazev = "Temno" },
                    new Kniha() { KnihaId = 3, Nazev = "Pan prstenu" }
                    );

                context.Ctenari.AddRange(
                    new Ctenar() { CtenarId = 1, Jmeno = "Petr" },
                    new Ctenar() { CtenarId = 2, Jmeno = "Erik" },
                    new Ctenar() { CtenarId = 3, Jmeno = "Alena" }
                    );

                context.SaveChanges();

                _databaseInitialized = true;
            }
        }
    }

    public KnihovnaContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<KnihovnaContext>()
            .UseSqlite("DataSource=test.db")
            .Options;

        return new KnihovnaContext(options);
    }
}
```