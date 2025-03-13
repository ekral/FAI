# 04 Testování Minimal Web API

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
    public DbSet<Kniha> Knihy { get; set; }
}
```

Dále si do projektu přidáme nový projekt s názvem "xUnit Test Project" a v tomto novém projektu přidáme referenci na projekt obsahující entity a DbContext. 

Nástroj xUnit organizuje testovací metody do tříd, kdy testovací metody ve třídě se spouští sekvenčně a testy v různých třídách se potom pouští souběžně. My budeme pracovat s produkční databází, tedy s typem databáze kterou budeme používat v produkci. Aby se testy vzájemně neovlivňovaly, tak by si teoreticky mohla každá testovací metoda vytvořit vlastní databázi. Prakticky by to ale výrazně zpomalilo provádění testů. Proto si vytvoříme jen jednu databázi, kterou budeme sdílet mezi všemi testovacími metodami. 

xUnit vytváří instanci testovací třídy pro každou metodu. Proto musíme použít [Class Fixture](https://xunit.net/docs/shared-context#class-fixture), což je způsob jak sdílet jednu instanci databáze mezi testovacími metodami v jedné třídě. Mohli bychom také použít [Collection Fixtures](https://xunit.net/docs/shared-context#class-fixture), ale to by zabránilo souběžnému provádění testovacích metod v různých třídách a zpomalilo by spouštění testů. Proto používáme kritickou sekci a sdílíme instanci DbContextu mezi více testovacími třídami. Pokud by se testy v třídách vzájemně ovlivňovaly, tak můžeme použít více databází a vytvořit více Class Fixtur pro různé třídy.

Následující kód představuje Class Fixture. Nakonfigurujeme a inicializujeme databázi specificky pro testy a nejprve databázi odstraníme a poté vytvoříme, abychom začínali pokaždé se stejnou databází.

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

Vlastní testy by potom vypadaly následovně. Kdy dědíme od `IClassFixture<DatabaseFixture>` a v konstruktoru (používáme primární konstruktor) získáme referenci na instanci `DatabaseFixture`, kterou si uložíme do property Fixture.

```csharp
public class UnitTestWebAPI(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private DatabaseFixture Fixture { get; } = fixture;

    [Fact]
    public async Task GetAllBooks_ShouldReturnAllBooks()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();

        // Act
        var result = await WebApiVersion1.GetAllBooks(context);

        // Assert
        Assert.Equal(3, result.Value?.Length);
    }

    [Fact]
    public async Task GetBook_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();

        // Act
        var result = await WebApiVersion1.GetBook(1, context);

        // Assert
        Ok<Kniha> okKniha = Assert.IsType<Ok<Kniha>>(result.Result);

        Assert.Equal(1, okKniha.Value?.KnihaId);
    }

    [Fact]
    public async Task GetBook_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();

        // Act
        var result = await WebApiVersion1.GetBook(999, context);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task CreateBook_ShouldAddBook()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();
        
        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        var newBook = new Kniha { KnihaId = 4, Nazev = "New Book" };

        // Act
        _ = await WebApiVersion1.CreateBook(newBook, context);

        context.ChangeTracker.Clear();

        // Assert
        Assert.Equal(4, await context.Knihy.CountAsync());
        Kniha? book = await context.Knihy.FindAsync(4);
        Assert.NotNull(book);
        Assert.Equal("New Book", book.Nazev);
    }

    [Fact]
    public async Task UpdateBook_ShouldUpdateBook_WhenBookExists()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        var updatedBook = new Kniha { Nazev = "Updated Book" };

        // Act
        var result = await WebApiVersion1.UpdateBook(1, updatedBook, context);

        context.ChangeTracker.Clear();

        // Assert
        Assert.IsType<NoContent>(result.Result);
        Kniha? book = await context.Knihy.FindAsync(1);
        Assert.NotNull(book);
        Assert.Equal("Updated Book", book.Nazev);
    }

    [Fact]
    public async Task UpdateBook_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        var updatedBook = new Kniha { Nazev = "Updated Book" };

        // Act
        var result = await WebApiVersion1.UpdateBook(999, updatedBook, context);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task DeleteBook_ShouldRemoveBook_WhenBookExists()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        // Act
        var result = await WebApiVersion1.DeleteBook(1, context);

        context.ChangeTracker.Clear();

        // Assert
        Assert.IsType<NoContent>(result.Result);
        Assert.Null(await context.Knihy.FindAsync(1));
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        using KnihovnaContext context = Fixture.CreateContext();

        context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

        // Act
        var result = await WebApiVersion1.DeleteBook(999, context);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }
}
```

Zajímavé je v předchozím kódu použití transakcí, pro to, aby se změny nezůstali po provedení testu v databází. Pokud transakci nepotvrdíme, tak se změny na konci metody odstraní z databáze. Pokud by některá z metod, které testujeme používala transakce, tak bychom mohli pro tento test vytvořit vlastní Class Fixture.