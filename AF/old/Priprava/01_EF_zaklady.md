# Entity framework

Návod pro Entity Framework: [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli). 

Poznámka: Zvolte minimálně **.NET 7**.

Budeme používat následující příkazy pro příkazovou řádku*:
- Příkaz ```dotnet add packgage``` stáhne nuget balíček z repozitáře nuget.org a přidá ho do projektu.
- Příkaz ```dotnet tool install```, který instaluje nové příkazy pro příkazovou řádku.
- Příkaz ```dotnet ef``` pomocí kterého vytváříme například nové migrace nebo aktualizujeme databázi.

*Příkazy musíme spouštět v Terminálu a v adresáři, kde je uložený *.csproj.

## Příklad

V následujícím příkladu definujteme třídu Student a pomocí migrací vytvoříme Sqlite databázi.

### 1. Definice třídy Student

Ve třídě nechceme mít parametrický konstruktor, což by nám mohlo komplikovat práci s Entity Frameworkem. Property představují sloupce tabulky a ```Id``` je dle jmenných konvencí primární klíč.

```csharp
public class Student
{
    public int Id { get; set; } // Primární klíč dle jmenných konvencí
    public required string Jmeno { get; set; }     
    public required string Prijmeni { get; set; }     
}
```

Použiváme proto klíčové slovo ```required```, které říká, že ```Jmeno``` a ```Prijmeni``` musi mit přiřazenou hodnotu nejpozději v Object Initializeru jak je ukázané v následujícím kódu.

```csharp
Student student = new()
{
    Jmeno = "Andrea",
    Prijmeni = "Nova"
};
```
## 2. DbContext a Entity Framework Provider

Pokud chceme používat konkrétní databázi s Entity Frameworkem, tak musím do projektu přidat providera pro tuto databázi. Provider je většinou knihovna distriovaná jako nuget balíček. Následující příkaz nainstaluje nuget balíček, konrétně EF database provider pro databázi Sqlite.

Příkaz musíte spustit v adresáři, kde se nachází projekt, tedy soubor s příponou *.csproj.

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

Dále definujeme potomka třídy ```DbContext``` a kolekce ```DbSet``` potom definuje tabulku v databázi.

```csharp
using Microsoft.EntityFrameworkCore;

public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }
}
```

Pomocí přetížené metody OnConfiguring potom nakonfigurujeme databázi, konrétně zadáme connection string. 

Jednoduchý zápis:

```csharp
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=studenti.db");
    }
}
```

Zápis, který ukládá databázi do dokumentů uživatele a používá connection string builder aby nedošlo k chybnému zápisu.

```csharp
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        string folderPath = Environment.GetFolderPath(folder);
        string filePath = Path.Join(folderPath, "studenti3.db");

        SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
        {
            DataSource = filePath
        };

        optionsBuilder.UseSqlite(csb.ConnectionString);
    }
}
```

Alternativně můžeme předat `DbContextOptions<ApplicationDbContext>` v konstruktoru. Díky tomu, můžeme mimo třídu zvolit i jiného providera a tedy používat jinou databázi. To je výhodné například při testování.



Dále přidáme metodu ```OnModelCreating```, kde můžeme zadat výchozí data v databází, ale také přesněji specifikovat primární klíče, cizí klíče a další.

```csharp
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        string folderPath = Environment.GetFolderPath(folder);
        string filePath = Path.Join(folderPath, "studenti3.db");

        SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
        {
            DataSource = filePath
        };

        optionsBuilder.UseSqlite(csb.ConnectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(
            new Student() { Id = 1, Jmeno = "Andrea", Prijmeni = "Nova"},
            new Student() { Id = 2, Jmeno = "Jiri", Prijmeni = "Novotny"},
            new Student() { Id = 3, Jmeno = "Karel", Prijmeni = "Vesely"}
        );
    }
}
```

## Migrace

Pomocí migrací můžeme vytvářet a aktualizovat databázi pomocí příkazů pro příkazovou řádku. Příkazy opět pouštějte v adresáři, kde se nachází projekt, tedy soubor s příponou *.csproj a před spuštěním nezapomeňte uložit všechny soubory.

### Příprava potřebných závislostí 

Proto, abychom mohli vytvářet nové migrace a aktualizovat databázi, tak musíme nainstalovat:
- nástroj **dotnet ef**. 
- Nuget balíček podporující vytváření migrací **Microsoft.EntityFrameworkCore.Design**

Následující příkaz nainstaluje příkaz **dotnet ef** globálně pro všechny projekty.

```powershell
dotnet tool install --global dotnet-ef
```

A následující příkaz přídá do projektu nuget balíček **Microsoft.EntityFrameworkCore.Design**.

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Vytváření a spuštění migrací

Migrace představuje kód v jazyce C# který umí například vytvářet nebo aktualizovat tabulky v databázi a případně i vložit výchozí data pro model. 

Následující příkaz **dotnet ef** vytvoří novou migraci s názvem *VychoziMigrace*. 

```powershell
dotnet ef migrations add VychoziMigrace
```

A následující příkaz migraci aplikuje a vytvoří novou databází, nebo zaktualizuje stávající.

```powershell
dotnet ef database update
```

Poznámka: bez použití migrací můžeme vytvořit databází pomocí metody *EnsureCreated() nebo EnsureCreatedAsync()*.

```csharp
await using SkolaContext db = new SkolaContext();
bool created = await db.Database.EnsureCreatedAsync();
```
