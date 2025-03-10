# 01 Základy Entity Frameworku

**autor: Erik Král ekral@utb.cz**

---

Entity Framework (EF) slouží k objektově relačnímu mapování. Což znamená, že můžeme pracovat s objekty v paměti a EF nám vygeneruje příkazy pro databází. Díky tomu také nejsme závislí na konkrétním typu databáze. S pomocí technologie **LINQ to Entities** (entita je třída reprezentující řádek tabulky v databázi) potom pracujeme s databází obdobným způsobem jako s objekty pomocí **LINQ to objects**.

Návod pro začátečníky Entity Framework: [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli) a podrobný přehled: [Creating and Configuring a Model](https://learn.microsoft.com/en-us/ef/core/modeling/)

V následujícím příkladu definujeme třídu Student a pomocí migrací vytvoříme Sqlite databázi.

## 1. Definice entity student Student

Nejprve si nadefinujeme třídu `Student`. Ve třídě nechceme mít parametrický konstruktor, což by nám mohlo komplikovat práci s Entity Frameworkem. Property představují sloupce tabulky a ```Id``` je dle jmenných konvencí primární klíč. Jako alternativu pro parametrický konstruktor v příkladu použiváme klíčové slovo ```required```, které říká, že ```Jmeno``` a ```Prijmeni``` musi mit přiřazenou hodnotu nejpozději v Object Initializeru.

```csharp
public class Student
{
    public int Id { get; set; } // Primární klíč dle jmenných konvencí
    public required string Jmeno { get; set; }     
    public required string Prijmeni { get; set; }     
}

Student student = new()
{
    Jmeno = "Andrea",
    Prijmeni = "Nova"
};
```

## 2. Definice DbContextu

Pokud chceme používat konkrétní databázi s Entity Frameworkem, tak musím do projektu přidat **database provider** pro tuto databázi. Database provider je knihovna distribuovaná jako nuget balíček. 

Například nuget balíček [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/microsoft.entityframeworkcore.sqlite) přidá do projektu podporu pro databází Sqlite.

Dále definujeme potomka třídy ```DbContext``` kde kolekce typu ```DbSet<Student>``` potom definuje tabulku v databázi s názvem `Students`.

```csharp
using Microsoft.EntityFrameworkCore;

public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }
}
```

Pomocí přetížené metody `OnConfiguring` potom nakonfigurujeme databázi, konrétně zadáme connection string. 

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

Alternativně můžeme předat `DbContextOptions<ApplicationDbContext>` v konstruktoru. Díky tomu můžeme mimo třídu zvolit i jiného providera a tedy používat jinou databázi. To je výhodné například při testování.

```csharp
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public StudentContext(DbContextOptions<StudentContext> options) : base(options)
    {

    }
}

using StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                        .UseSqlite("Data Source=studenti.db")
                                        .Options);
```

---
Poznámka: Pro vytvoření connection stringu můžeme použít `SqliteConnectionStringBuilder` tak aby nedošlo k chybnému zápisu. V příkladu také volíme umístění souboru Sqlite databáze do dokumentů uživatele.

```csharp
using System.IO;
using Microsoft.Data.Sqlite;

var folder = Environment.SpecialFolder.MyDocuments;
string folderPath = Environment.GetFolderPath(folder);
string filePath = Path.Join(folderPath, "studenti3.db");

SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
{
    DataSource = filePath
};

string connectionString = csb.ConnectionString;
```

Dále můžeme přidat metodu ```OnModelCreating```, kde můžeme zadat výchozí data v databází, ale také přesněji specifikovat primární klíče, cizí klíče a další.

```csharp
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=studenti.db");
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

## 3. Vytvoření databáze

Databázi vytvoříme buď příkazem `EnsureCreated`, což se používá pro vývoj. Pokud databáze neexistuje, tak příkaz databázi vytvoří.

Vytvoření příkazu pomocí `EnsureCreated`:


```csharp
using StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                        .UseSqlite("Data Source=studenti.db")
                                        .Options);

context.Database.EnsureCreated();
```

Na cvičení budeme používat tento postup, ale jinak můžeme databázi vytvořit i pomocí nástrojů pro příkazovou řádku, což probereme příště.

Databázi (soubor studenti.db) si můžeme prohlédnout například v [SQLite Viewer Web App](https://sqliteviewer.app).

## 4. Práce s databází

S databází pracujeme pomocí  [LINQ to Entities](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/linq-to-entities).

### Nový řádek databáze

Následující kód představuje ukázku přidání nového řádku do tabulky studentů. Všimněte si, že když vytváříme instanci třídy `Student`, tak nezadáváme hodnotu property `Id` a ta bude mít tedy hodnotu `0`. Potom co vložíme nového studenta pomocí příkazu `context.Add(novy)` a zavoláme příkaz `context.SaveChanges()`, tak se property `novy.Id` nastaví na vygenerovanou hodnotu primárního klíče. Příkaz `context.SaveChanges()` také vrací počet změněných řádků, v tomto případě vrátí hodnotu `1` protože jsme změnili jeden řádek.

```csharp
Student novy = new Student() { Jmeno = "Jiri", Prijmeni = "Vesely" };

context.Add(novy);

int number = context.SaveChanges();

Console.WriteLine($"Pocet entit zapsanych do databaze: {number}");

Console.WriteLine($"Vygenerovane Id: {novy.Id}");
```

### Změna řádku tabulky

Následující kód změní řádek tabulky (entitu) v databázi. Konkrétně změní řádek studenta s `Id` `1`.

```csharp
Student studentUpdate = new Student() { Id = 1, Jmeno = "Karel", Prijmeni = "Jiny" };
context.Students.Update(studentUpdate);

context.SaveChanges();
```

### Získání všech řádků tabulky

Všechny řádky tabulky získám tak, že například použiji `foreach` nebo metodu `ToList` nebo `ToArray`. Při provedení těchto příkazů se provede dotaz do databáze.

```csharp
foreach (Student student in context.Students)
{
    Console.WriteLine($"{student.Id} {student.Jmeno} {student.Prijmeni}");
}

List<Student> studentiList = context.Students.ToList();
Student[] studentiArray = context.Students.ToArray();
```

### Filtrování prvků

Následující příkaz vrátí všechny studenty s příjmením `"Vesely"`. Všimněte si návratového typu `IQueryable<Student>` nad kterým můžeme definovat dotazy. Vlastní dotaz se provede až po spuštění příkazu `foreach` nebo kdybychom zavolali příkaz `ToList` a podobně.

```csharp
IQueryable<Student> students = context.Students.Where(s => s.Prijmeni == "Vesely");

foreach(Student student in students)
{
    Console.WriteLine($"{student.Id} {student.Jmeno} {student.Prijmeni}");
}
```

### Nalezení prvku podle primárního klíče

Následující příkaz vrátí studenta podle hodnoty primárního klíče.

```csharp
int id = 1;

Student? student = context.Students.Find(id);

if (student is not null)
{
    Console.WriteLine($"{student.Id} {student.Jmeno} {student.Prijmeni}");
}
```

### Nalezení prvního prvku splňující podmínku

Následující příkaz vrátí studenta jehož jméno začíná na `"Nov"`. Pokud žádný záznam podmínku nesplní, tak příkaz vrátí `null`.

```csharp
Student? studentByPrijmeni = context.Students.FirstOrDefault(s => s.Prijmeni.StartsWith("Nov"));

if (studentByPrijmeni is not null)
{
    Console.WriteLine($"{studentByPrijmeni.Id} {studentByPrijmeni.Jmeno} {studentByPrijmeni.Prijmeni}");
}
```

### Projekce

Projekce představuje změnu typu než je orignální typ entity v databázi. Například následující příkaz vrátí jen jména studentů. Místo typu `Student` tedy vrací `string`. Metoda opět vrací IQueryable, což znamená, že se dotaz do databáze se neprovede hned, ale teprve až provedeme například `foreach`.

```csharp
IQueryable<string> jmena = context.Students.Select(s => s.Jmeno);

foreach (string jmeno in jmena)
{
    Console.WriteLine(jmeno);
}
```

### Řazení

Entity můžeme řadit vzestupně nebo sestupně a to buď pomocí výchozího řazení dle klíče nebo pomocí vlastního řazení. První příkaz seřadí studenty dle klíče vzestupně, druhý příkaz seřadí studenty dle klíče sestupně. A třetí příkaz seřadí studenty dle příjmení vzestupně. Všimněte si, že návratový typ je tentokrát `IOrderedQueryable`.

```csharp
IOrderedQueryable<Student> serazeniStudentiDleKliceVzestupne = context.Students.Order();
IOrderedQueryable<Student> serazeniStudentiDleKliceSestupne = context.Students.OrderDescending();
IOrderedQueryable<Student> serazeniStudentiPodlePrijmeniVzestupne = context.Students.OrderBy(s => s.Prijmeni);
```
### Kombinace metod

Metody můžeme kombinovat. Následující příkaz vrací jména studentů s příjmením `"Vesely"` (filtruje) seřazená vzestupně. Dotaz se opět neprovede hned, ale až bychom provedli například příkaz `foreach` nebo `ToList`.

```csharp
IOrderedQueryable<string> jmena = context.Students
    .Where(s => s.Prijmeni == "Vesely")
    .Select(s => s.Jmeno)
    .OrderDescending();
```

Protože návrazové typy můžou být složité, tak se často používá klíčové slovo `var`, předchozí příkaz s použitím `var` by vypadal následovně:

```c#
var jmena = context.Students
    .Where(s => s.Prijmeni == "Vesely")
    .Select(s => s.Jmeno)
    .OrderDescending();
```


---
Úkoly:

1) Vytvořte vlastní příklad na One to One relation.
2) Vytvořte vlastní příklad na One to Many relation.
3) Vytvořte vlastní příklad na Many to Many relation.
