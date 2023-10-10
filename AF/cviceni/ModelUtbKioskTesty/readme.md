# Úkol na cvičení: Testy pro Model Utb.Kiosk

Otestujte databází vytvořenou ve cvičení [Model Utb.Kiosk](../ModelUtbKiosk).

Vyjděte z předmětu:

- Nástroje pro vývoj softwarových projektů.
  
Otestujte funkčnost modelu, kdy můžete využít i [xUnit testy](https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database), vyjděte z příkladu na [Utb.Studenti](https://github.com/ekral/FAI/tree/master/AF/src/Utb.Studenti).

Cílem je otestovat, zda je databáze správně vytvořená a zda jsou správně nastavené relace. Tedy to, co jste předtím vytvářeli v konzolové aplikaci a jen jste se podívali na výstup na konzoli. Tento postup chceme zautomatizovat v testech.

1) Vytvořte nový projekt *Utb.PizzaKiosk.Models* typu **class library** ve verzi minimálně .NET 7. Název Solution bude *Utb.PizzaKiosk*.
2) Podle [návodu v přípravě](https://github.com/ekral/FAI/blob/master/AF/Priprava/01_EF_zaklady.md) proveďte do projektu *Utb.PizzaKiosk.Models* přidejte modely pro Pizza Kiosk a DbContext a vytvořte migraci.
3) Do solution přidejte nový projekt typu **xUnit test** s názvem *Utb.PizzaKiosk.Tests*.
4) Do projektu *Utb.PizzaKiosk.Tests* přidejte referenci na projekt *Utb.PizzaKiosk.Models*.
5) Vytvořte testy dle následujícího textu.

Pro xUnit testy můžeme vytvořit DatabaseFixture protože chceme aby se databáze vytvořila jenom jednou a byla bezpečně vytvořená i v rámci testů spuštěných ve více vláknech, k tomu nám slouží ```CollectionDefinition```.

```csharp
[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<TestDatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class TestDatabaseFixture
{
    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    // add test only data
                }

                _databaseInitialized = true;
            }
        }
    }

    public StudentContext CreateContext() => new StudentContext("test.db");
}
```

Database context je upravený tak, aby bylo možné pro test zadávat jiný název souboru. Existuje více řešení, například pomocí dědičnosti, nebo parametrického konstruktoru. Ale pro tento případ jsem zvolil jen název souboru.

```csharp
 public class StudentContext : DbContext
    {
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Skupina> Skupiny { get; set; }

        private string dbPath = "studenti.db";

        public StudentContext()
        {

        }

        public StudentContext(string dbPath)
        {
            this.dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            string folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, dbPath);

            SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
            {
                DataSource = filePath
            };

            optionsBuilder.UseSqlite(csb.ConnectionString);
        }

    }
```

V testech potom použiji `TestDatabaseFixture` a `Collection` "Database collection".

```csharp
[Collection("Database collection")]
public class StudentContextTest : IClassFixture<TestDatabaseFixture>
{
    public StudentContextTest(TestDatabaseFixture fixture) => Fixture = fixture;

    public TestDatabaseFixture Fixture { get; }

    [Fact]
    public void StudentJeBohumil()
    {
        using StudentContext context = Fixture.CreateContext();

        Student student = context.Studenti.Single(s => s.Id == 1);

        Assert.Equal("Bohumil", student.Jmeno);
    }

    [Fact]
    public void BohumilMaTelocvik()
    {
        using StudentContext context = Fixture.CreateContext();

        Student student = context.Studenti.First(s => s.Id == 1 && s.Predmety.Any(p => p.Nazev == "Telocvik"));

        Assert.Equal("Bohumil", student.Jmeno);
    }
}
```

Kolekci můžete v xUnit otestovat následující způsobem. Pro každý prvek kolekce můžeme použít lambda výraz, `s1` je v tomto případě první student v kolekci, `s2` je druhý student v kolekci.

```csharp
Assert.Collection(studenti.Value, 
s1 =>
{
    Assert.Equal(1, s1.Id);
    Assert.Equal("Bohumil", s1.Jmeno);
}, 
s2 =>
{
    Assert.Equal(2, s2.Id);
    Assert.Equal("Stefan", s2.Jmeno);
});
```

---
Tutoriály

- [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli).
- [Testing against your production database system](https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database).
