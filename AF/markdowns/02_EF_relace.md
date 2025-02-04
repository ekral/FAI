# 02 Relace v entity frameworku

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu probereme relace one to one (1 : 1), one to many (1 : n) a many to many (n : m).

## Relace One to One

V následujícím příkladu si ukážeme příklad na relaci one to one. Budeme mít třídu Student a StudentCart, kdy student může mít jen jednu studentskou kartu a studentská karta bude patřit jen jednomu studentovi.

V příkladu bude `Student` hlavní entita a `StudentCart` závislá entita.

`Student` má
- Navigation property `StudentCart`.

`StudentCart`má
- Cizí klíč `StudentId`.
- Navigation property `Student`.

```csharp
public class Student
{
    public int StudentId { get; set; }
    public required string Name { get; set; } 
    public StudentCart? StudentCart { get; set; } // Navigation Property to dependent

}
public class StudentCart
{
    public int StudentCartId { get; set; }
    public required DateTime PlatnostDo { get; set; }
    public required int StudentId { get; set; } // Cizí klíč s unique indexem
    public Student? Student { get; set; }
}
```

Vše se nakonfiguruje s pomocí jmenných konvencí. Property `StudentId` bude cizí klíč a unique index (`CREATE UNIQUE INDEX "IX_Carts_StudentId" ON "Carts" ("StudentId")`). 

Data kontext bude vypadat následovně:

```csharp
class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCart> Carts { get; set; }

    public StudentContext(DbContextOptions<StudentContext> options) : base(options)
    {
        
    }
}
```

V tomto případě to není nutné, ale pro větší názornost si ukážeme jak bychom nakonfigurovali relace pomocí fluent API:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
        .HasOne(s => s.StudentCart)
        .WithOne(sc => sc.Student)
        .HasForeignKey<StudentCart>(sc => sc.StudentId)
        .IsRequired();

    modelBuilder.Entity<StudentCart>().HasIndex(sc => sc.StudentId).IsUnique();
}
```

Poznámka: Předchozí příklad by šel vyřešit i pomocí sdíleného primárního klíče.

## Relace One to Many

V následujícím příkladu budeme předpokládat, že student může být zapsaný jen v jedné studijní skupině. Student obsahuje cizí klíč `SkupinaId` a navigační property Skupina. Navigační property slouží čistě pro procházení dat a nepoužívá se pro přidání nového řádku nebo aktualizaci řádku. Pro přidání řádku nebo aktualizaci se používá cizí klíč.

Třída `Skupina` potom obsahuje jen navigační property `Studenti`. Tato properta opět slouží pro procházení dat.

Všimněte si, že navigační property `Skupina` i `Studenti` jsou **nullable**. Jejich hodnota tedy může být null a to napříkad, když budeme vytvářet nové řádky tabulek. Jako alternativu pro ostatní property kromě primárního klíče potom použijeme klíčové slovo `required`.


```csharp
public class Student
{
    public int StudentId { get; set; } // Primární klíč dle jmenných konvencí
    public required string Jmeno { get; set; }
    public required string Prijmeni { get; set; }
    public int SkupinaId { get; set; } // Cizí klíč
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

V následujících příklad si probereme vytvoření řádku s relacemi a načtení dat pomocí dvou ruzných způsobů. Aby byl kód přehlednější, tak si nadefinujeme pomocnou metodu pro vytváření DbContextu:

```csharp
static StudentContext CreateContext()
{
    StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                            .UseSqlite("Data Source=studenti.db")
                                            .Options);
    return context;
}
```

### Nový řádek databáze

Následující kód představuje ukázku naplnění databáze výchozími daty. Vytvoříme jednu skupinu, do které přidáme dva studenty. Všimněte si, že zadáváme rovnou hodnoty primárních klíčů. Co je důležité z hlediska relací, tak že zadáváme hodnotu cizího klíče `SkupinaId` a nezadáváme hodnoty navigačních property, které slouží pouze pro čtení a při vytváření nových řádků nemají žádný význam.


```csharp
using StudentContext context = CreateContext();

Skupina skupina = new Skupina() { SkupinaId = 1, Nazev = "SWI1" };
Student student1 = new Student() { StudentId = 1, SkupinaId = 1, Jmeno = "Jiri", Prijmeni = "Pokorny" };
Student student2 = new Student() { StudentId = 2, SkupinaId = 1, Jmeno = "Alena", Prijmeni = "Dulikova" };

context.Skupiny.Add(skupina);
context.Studenti.AddRange(student1, student2);

context.SaveChanges();
```

### Načtení dat

Při načtení dat můžeme načíst entity bez navigačních propert a nebo navigační property načíst přičemž máme [tři možnosti](https://learn.microsoft.com/en-us/ef/core/querying/related-data/):

#### 1. Eager loading

Pomocí metody `Include` v dotazu říkáme, která navigační property se má načíst.

Pokud bychom načetli skupinu následujícím způsobem, tak navigační property `skupina.Studenti` by byla `null`.

```csharp
var skupiny = context.Skupiny;

foreach(Skupina skupina in skupiny)
{
    // skupina.Studenti bude null
}
```

Když ale použijeme metodu `Include`, tak se v dotazu zahrne dotaz i na studenty a načtou se všichni studenti pro každou skupinu.

```csharp
var skupinySeStudenty = context.Skupiny.Include(skupina => skupina.Studenti);

foreach (Skupina skupina in skupiny)
{
    Console.WriteLine($"Skupina {skupina.SkupinaId}: {skupina.Nazev}");

    if (skupina.Studenti is not null)
    {
        foreach (Student student in skupina.Studenti)
        {
            Console.WriteLine($"Student {student.StudentId}: {student.Jmeno} {student.Prijmeni}");
        }
    }
}
```

Pokud chceme načíst navigation property pro includovanou property, tak můžeme použít metodu [ThenInclude](https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#including-multiple-levels). Entity framework dále podporuje [filtrování entit](https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#filtered-include), která načítáme v metodě `Include`. Dále můžeme nakonfigurovat context s pomocí [AutoInclude](https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations) tak, aby se navigační property načítaly automaticky.

Je potředa si ale uvědomit, že Eager loading může mít **negativní vliv** na výkon. Kdy objem načtených dat při každém dalším vnoření může růst exponenciálně. 

#### 2. Explicit Loading

U [Explicit Loading](https://learn.microsoft.com/en-us/ef/core/querying/related-data/explicit#explicit-loading) najprve provedeme dotaz a teprvé potom dodatečně načteme související navigation property. Konkrétně použijeme metody `DbContext.Entry(...)` API, kokrétně metody Collection jak je ukázáno v následujícím příkladu, kdy navigation property `skupina.Studenti` je kolekce.

```csharp
Skupina skupina = context.Skupiny.Single(s => s.SkupinaId == 1);

if (skupina.Studenti is null)
{
    Console.WriteLine("Studenti jsou zatím null");
}

context.Entry(skupina).Collection(skupina => skupina.Studenti).Load();

if (skupina.Studenti is not null)
{
    foreach (Student student in skupina.Studenti)
    {
        Console.WriteLine($"Student {student.StudentId}: {student.Jmeno} {student.Prijmeni}");
    }
}
```

A nebo metodu `Reference`, kdy navigation property `Student.Skupina` není kolekce, ale reference na jedn Skupinu.

```csharp
Student student = context.Studenti.Single(student => student.StudentId == 1);

if(student.Skupina is null)
{
    Console.WriteLine("Skupina je zatím null.");
}

context.Entry(student).Reference(student => student.Skupina).Load();

if (student.Skupina is not null)
{
    Console.WriteLine($"Skupina {student.Skupina.SkupinaId}: {student.Skupina.Nazev}");
}
```

#### 2. Lazy Loading

Contex je možné taky nakonfigurovat, aby využíval [Lazy Loading](https://learn.microsoft.com/en-us/ef/core/querying/related-data/lazy) a načítal data automaticky, když k nim přistupujem. 