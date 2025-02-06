# 02 Relace v entity frameworku

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu probereme relace one to many (1 : n), one to one (1 : 1),  a many to many (n : m).


U relací v Entity Frameworku rozlišujeme:

- **Cizí klíč** který slouží pro definici relace a ukládá se do databáze.
- **Navigační property** a to buď jako referenci nebo kolekci referencí, která slouží především pro procházení objektů v paměti.

## Relace one to many

V následujícím příkladu budeme mít studenty a studijní skupiny a budeme předpokládat, že student může být zapsaný jen v jedné studijní skupině. 

Student obsahuje:
- cizí klíč `SkupinaId` a 
- navigační property `Skupina`. 

Třída `Skupina` potom obsahuje:
- collection navigační property `Studenti`.

Jako alternativu pro konstruktor použijeme klíčové slovo `required`. Všimněte si, že navigační property `Skupina` i `Studenti` jsou **nullable**, jejich hodnota tedy může být null.

```csharp
public class Student
{
    public int StudentId { get; set; } 
    public required string Jmeno { get; set; }
    public required string Prijmeni { get; set; }
    public int SkupinaId { get; set; } // Cizí klíč
    public Skupina? Skupina { get; set; } // Navigation Property
}

public class Skupina
{
    public int SkupinaId { get; set; }
    public required string Nazev { get; set; } 
    public ICollection<Student>? Studenti { get; set; } // Collection Navigation Property
}
```

Dále vytvoříme DbContext:

```csharp
public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
{
    public DbSet<Student> Studenti { get; set; }
    public DbSet<Skupina> Skupiny { get; set; }
}
```

Vše se nakonfiguruje pomocí jmenných konvencí.

V tomto případě to tedy není nutné, ale pro větší názornost si ukažeme jak bychom nakonfigurovali relace pomocí fluent API:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
                .HasOne(s => s.Skupina)
                .WithMany(sk => sk.Studenti)
                .HasForeignKey(s => s.SkupinaId);
}
```

### Nový řádek databáze

V následujících příkladech probereme vytvoření řádku s relacemi a načtení dat pomocí dvou ruzných způsobů. Aby byl kód přehlednější, tak si nadefinujeme pomocnou metodu pro vytváření DbContextu:

```csharp
static StudentContext CreateContext()
{
    StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                            .UseSqlite("Data Source=studenti.db")
                                            .Options);
    return context;
}
```

V následujícím kódu vytvoříme jednu skupinu, do které přidáme dva studenty. Všimněte si, že zadáváme rovnou hodnoty primárních klíčů. Důležité z hlediska relací je, že zadáváme hodnotu cizího klíče `SkupinaId`. Hodnoty navigačních propert nezadáváme.

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

Pokud načítáme data z databáze, tak ve výchozím nastavení se navigation property nenačítají.

Pokud bychom tedy načetli skupinu následujícím způsobem, tak by navigation property `skupina.Studenti` byla `null`:

```csharp
var skupiny = context.Skupiny;

foreach(Skupina skupina in skupiny)
{
    // skupina.Studenti bude null
}
```

Máme [tři možnosti](https://learn.microsoft.com/en-us/ef/core/querying/related-data/) jak navigační property načíst:

#### 1. Eager loading

Pomocí metody `Include` v dotazu říkáme, která navigační property se má načíst.

V následujícm příkladu použijeme metodu `Include` tak by se načetli všichni studenti pro každou skupinu.

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

U [Explicit Loading](https://learn.microsoft.com/en-us/ef/core/querying/related-data/explicit#explicit-loading) najprve provedeme dotaz a teprvé potom dodatečně načteme související navigation property. Použijeme metody `DbContext.Entry(...)` API, konkrétně metodu `Collection` jak je ukázáno v následujícím příkladu, kdy navigation property `skupina.Studenti` je kolekce.

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

Dále můžeme použit metodu `Reference` pro navigation property, které není kolekce., Například navigation property `Student.Skupina` není kolekce, ale reference na jednu skupinu.

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

#### 2. Lazy loading

Contex je možné taky nakonfigurovat, aby využíval [Lazy Loading](https://learn.microsoft.com/en-us/ef/core/querying/related-data/lazy) a načítal data automaticky, když k nim přistupujeme. 

## Relace one to one

V následujícím příkladu si ukážeme příklad na relaci one to one. Budeme mít třídu `Student` a `StudentCart`, kdy student může mít jen jednu studentskou kartu a studentská karta může patřit jen jednomu studentovi.

V příkladu bude `Student` hlavní entita a `StudentCart` závislá entita.

`Student` má
- Navigation property `StudentCart`.

`StudentCart` má
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
    public Student? Student { get; set; } // Navigation Property to principal entity
}
```

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

Vše se nakonfiguruje s pomocí jmenných konvencí. Property `StudentId` bude cizí klíč a unique index (`CREATE UNIQUE INDEX "IX_Carts_StudentId" ON "Carts" ("StudentId")`). 

V tomto případě to tedy není nutné, ale pro větší názornost si opět ukažeme jak bychom nakonfigurovali relace pomocí fluent API:

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

## Relace many to many

Relaci many to many si ukážeme na příkladů studentů a předmětů, kdy student má více předmětů a předmět může mít více studentů.

### Basic many to many

Nejprve si ukážeme variantu [Basic many-to-many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#basic-many-to-many)

Definujeme si třídy `Student` a `Subject`. 

`Student` má:
- Collection navigation property `Subjects`.

`Subject` má:
- Collection navigation property `Students`.


```csharp
class Student
{
    public int StudentId { get; set; }
    public required string Jmeno { get; set; }
    public List<Subject>? Subjects { get; set; }
}

class Subject
{
    public int SubjectId { get; set; }
    public required string Name { get; set; }
    public List<Student>? Students { get; set; }
}
```
DbContext bud vypadat následovně:

```csharp
class StudentContext : DbContext
{
    public DbSet<Student> Studenti { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    public StudentContext(DbContextOptions<StudentContext> options) : base(options)
    {
        
    }
}
```
A to je vše co potřebujeme, dle jmenných konvencí vytvoří entity framework vytvoří propojovací tabulku `StudentSubject` v databázi automaticky.

Nový záznam vytoříme například následujícím způsobem. Všimněte si, že subject nepřidáváme do tabulky, ale jen do navigation property `student.Subjects`, ale i tak se subject přidá do tabulky `Subjects` a také se přidá záznam do tabulky `StudentSubject`. 

```csharp
using StudentContext context = CreateContext();

if (context.Database.EnsureCreated())
{
    Student student = new Student() { StudentId = 1, Jmeno = "Karl" };
    Subject subject = new Subject() { SubjectId = 1, Name = "Math" };

    student.Subjects = [subject];
    
    context.Add(student);

    int count = context.SaveChanges();
}
```

### Many-to-many with class for join entity

U varianty [Many-to-many with class for join entity](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-with-class-for-join-entity) si přímo nadefinujeme propojovací třídu `StudentSubject` a označíme ji pomocí Fluent API. Výhodou je, že můžeme snadněji zadávat její hodnoty.

Budeme mít tedy následující třídy, kdy proti předchozímu příkladu přibyla třída `StudentSubject`.

```csharp
class Student
{
    public int StudentId { get; set; }
    public required string Jmeno { get; set; }
    public List<Subject>? Subjects { get; set; }
}

class Subject
{
    public int SubjectId { get; set; }
    public required string Name { get; set; }
    public List<Student>? Students { get; set; }
}

class StudentSubject
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
}
```

DbContext potom bude vypadat následovně. Propojovací join entita je definována metodou UsingEntity<StudentSubject>().

```csharp
class StudentContext : DbContext
{
    public DbSet<Student> Studenti { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    public StudentContext(DbContextOptions<StudentContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasMany(student => student.Subjects)
            .WithMany(skupina => skupina.Students)
            .UsingEntity<StudentSubject>();
    }
}
```