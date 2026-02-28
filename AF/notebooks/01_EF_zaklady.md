# 01 – Základy Entity Framework Core

**autor: Erik Král ekral@utb.cz**

## 🎯 Cíle kapitoly

Po prostudování této kapitoly budete schopni:

- vysvětlit pojem ORM
- vysvětlit, co je LINQ a jak funguje
- rozlišit query syntax a method syntax
- vysvětlit princip extension metod
- vytvořit jednoduchou entitu
- nakonfigurovat DbContext
- připojit se k SQLite databázi
- provádět základní CRUD operace
- vysvětlit rozdíl mezi IQueryable a IEnumerable
- pochopit princip odloženého vykonání dotazu

---

# 1. Co je ORM?

**ORM (Object-Relational Mapper)** je nástroj, který převádí:

- objekty v programovacím jazyce  
↕  
- tabulky v relační databázi  

Bez ORM musíme psát SQL:

```sql
SELECT * FROM Students WHERE Age > 20;
```

S ORM pracujeme objektově:

```csharp
var students = context.Students
                      .Where(s => s.Age > 20)
                      .ToList();
```

ORM zajistí:

1. překlad LINQ do SQL  
2. odeslání dotazu do databáze  
3. převod výsledku zpět na objekty  

---

# 2. Co je Entity Framework Core?

**Entity Framework Core (EF Core)** je ORM framework pro .NET.

Umožňuje:

- mapování tříd na tabulky  
- práci s daty pomocí LINQ  
- sledování změn objektů  
- automatické generování databáze  
- migrace schématu databáze  

---

# 3. Co je LINQ?

**LINQ (Language Integrated Query)** je součást jazyka C#, která umožňuje psát dotazy přímo v jazyce.

LINQ lze používat:

- nad kolekcemi v paměti (List<T>)
- nad databází (EF Core)  

---

## 3.1 Dva způsoby zápisu LINQ

### Query syntax (SQL-like)

```csharp
var result =
    from s in context.Students
    where s.Age > 18
    orderby s.Name
    select s;
```

### Method syntax (extension metody)

```csharp
var result = context.Students
                    .Where(s => s.Age > 18)
                    .OrderBy(s => s.Name)
                    .ToList();
```

> Method syntaxe je v praxi běžnější, ale pro složitější dotazy můžeme použít i query syntaxi.

---

## 3.2 Extension metody

Extension metoda:

- je statická metoda  
- rozšiřuje existující typ  
- používá klíčové slovo `this` v parametru  

Příklad:

```csharp
public static class StudentExtensions
{
    public static bool IsAdult(this Student student)
    {
        return student.Age >= 18;
    }
}
```

Použití:

```csharp
var adults = context.Students
                    .Where(s => s.IsAdult())
                    .ToList();
```

Metody jako `Where()`, `Select()`, `OrderBy()`, `FirstOrDefault()`, `Any()`, `Count()` jsou extension metody nad `IQueryable<T>` nebo `IEnumerable<T>`.

---

## 3.3 LINQ to Objects vs LINQ to Entities

### LINQ to Objects

```csharp
var list = new List<Student>();
var result = list.Where(s => s.Age > 18);
```

- běží v paměti  
- filtruje již načtená data  

### LINQ to Entities (EF Core)

```csharp
var result = context.Students
                    .Where(s => s.Age > 18);
```

- nevykoná se okamžitě  
- přeloží se do SQL  
- běží v databázi 

---

## 3.4 Odložené vykonání (Deferred Execution)

```csharp
var query = context.Students.Where(s => s.Age > 18);
```

Dotaz se vykoná až při volání například následujících metod:

```csharp
ToList();
FirstOrDefault();
Count();
Any();
```

Tomu se říká **materializace výsledku**.

---

## 3.5 IQueryable vs IEnumerable

| Typ | Kde běží | Kdy se vykoná |
|------|----------|---------------|
| `IQueryable<T>` | databáze | při materializaci |
| `IEnumerable<T>` | paměť | při iteraci |

EF Core používá `IQueryable<T>`.

---

# 4. Definice entity

```csharp
public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Age { get; set; }
}
```

- `Id` je primární klíč  
- instance odpovídá řádku v tabulce  
- EF sleduje změny objektů  

> required znamená, že musíme zadat hodnotu při vytváření instance třídy
---

# 5. DbContext

Do projektu definující DbContext musíme přidat providera pro Entity Framework Core, například nuget balíček `Microsoft.EntityFrameworkCore.Sqlite`.
Více se dozvíte například v tutoriálu [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli).

```csharp
public class StudentContext(DbContextOptions<StudentContext> options): DbContext(options)
{
    public DbSet<Student> Students { get; set; }
}
```

`DbSet<Student>` reprezentuje tabulku a umožňuje psát LINQ dotazy.

---

## 5.1 Přetížené metody DbContext

### OnConfiguring

Metoda `OnConfiguring` se používá k nastavení `DbContext` přímo v třídě, například pro konfiguraci poskytovatele databáze a connection stringu.  
Lze ji použít místo předávání `DbContextOptions` zvenčí.

```csharp
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=students.db");
        }
    }
}
```

Použití:

```csharp
using var context = new StudentContext();
context.Database.EnsureCreated();
```

Výhody a nevýhody:

- **Výhoda:** nemusíme pokaždé předávat `DbContextOptions`  
- **Nevýhoda:** méně flexibilní při testování

---

### OnModelCreating

Metoda `OnModelCreating` umožňuje konfiguraci mapování entit na tabulky, nastavení primárních klíčů, délky stringů a relací.

#### Příklad – konfigurace primárního klíče a délky stringu

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);

    modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();
}
```

---

### Shrnutí

| Metoda            | Účel |
|------------------|------|
| `OnConfiguring`   | Konfigurace DbContext (provider, connection string) |
| `OnModelCreating` | Konfigurace entit, relací, omezení, pravidel mapování |


> Tyto metody lze kombinovat s `DbContextOptions`, pokud chceme maximální flexibilitu.

---

# 6. Vytvoření databáze

```csharp
var options = new DbContextOptionsBuilder<StudentContext>()
                    .UseSqlite("Data Source=students.db")
                    .Options;

using var context = new StudentContext(options);
context.Database.EnsureCreated();
```

`EnsureCreated()` není vhodné pro produkci. V produkci používáme migrace, kdy si můžeme vygenerovat i SQL kód a ten potom nezávisle spustit.

---

# 7. CRUD operace

## CREATE

Když necháme `Id` s hodnotou `0`, tak Entity Framework, po volání metody `SaveChange`, **přidělí `Id` vygenerovanou hodnotu primárního klíče**.

```csharp
var student = new Student
{
    Name = "Jan",
    Age = 22
};

context.Students.Add(student);
context.SaveChanges();

int key = student.Id;
```

---

## READ

```csharp
var students = context.Students.ToList();

var older = context.Students
                   .Where(s => s.Age > 20)
                   .ToList();

var student = context.Students.Find(1); // Kód vyhledá studenta podle primárního klíče

var jan = context.Students
                 .FirstOrDefault(s => s.Name == "Jan");
```

---

## UPDATE

```csharp
Student? student = context.Students.Find(1);

if(student is not null)
{
    student.Age = 23;
    context.SaveChanges();
}
```

---

## DELETE

```csharp
Student? student = context.Students.Find(1);

if(student is not null)
{
    context.Students.Remove(student);
    context.SaveChanges();
}
```

---

# 8 Projekce

Projekce představuje změnu typu než je originální typ entity v databázi. Například následující příkaz vrátí jen jména studentů. Místo typu `Student` tedy vrací `string`. Metoda opět vrací `IQueryable`, což znamená, že se dotaz do databáze se neprovede hned, ale teprve až provedeme například `foreach`.

```csharp
IQueryable<string> jmena = context.Students.Select(s => s.Jmeno);

foreach (string jmeno in jmena)
{
    Console.WriteLine(jmeno);
}
```

---

# 9 Řazení

Metody pro řazení vrací typ `IOrderedQueryable`.

```csharp
var podleKliceVzestupne = context.Students.OrderBy(s => s.Id);
      
var podleKliceSestupne = context.Students.OrderByDescending(s => s.Id);
    
var podlePrijmeniVzestupne = context.Students.OrderBy(s => s.Prijmeni);
```

---

# 10. Kombinace metod

Metody můžeme kombinovat. Následující příkaz vrací jména studentů s příjmením `"Vesely"` (filtruje) seřazená sestupně. Dotaz se opět neprovede hned, ale až bychom provedli například příkaz `foreach` nebo `ToList`.

```csharp
IOrderedQueryable<string> jmena = context.Students
    .Where(s => s.Prijmeni == "Vesely")
    .OrderByDescending(s => s.Prijmeni)
    .Select(s => s.Jmeno);
```

---

# 11. Asynchronní přístup

V reálném kódu používáme většinou asynchronní varianty, například:

```csharp
var students = await context.Students.ToListAsync();
await context.SaveChangesAsync();
```

---

# 12. Překlad LINQ do SQL

```csharp
context.Students
       .Where(s => s.Age > 18)
       .OrderBy(s => s.Name)
       .Select(s => s.Name)
       .ToList();
```

Přibližný SQL dotaz:

```csharp
SELECT Name
FROM Students
WHERE Age > 18
ORDER BY Name;
```

---

# 10. Nejčastější chyby

- zapomenuté SaveChanges()
- předčasné volání ToList()
- nepochopení odloženého vykonání
- použití EnsureCreated() v produkci

---

# 11. Praktické úkoly

1. Vytvořte entitu `Course` (Id, Title, Credits).  
2. Přidejte 3 kurzy do databáze.  
3. Vypište kurzy s více než 3 kredity.  
4. Seřaďte kurzy podle názvu.  
5. Aktualizujte jeden kurz.  
6. Odstraňte jeden kurz.  

---

# ❓ Kontrolní otázky

1. Co je ORM?  
2. Jaký je rozdíl mezi LINQ to Objects a LINQ to Entities?  
3. Co je extension metoda?  
4. Co znamená odložené vykonání?  
5. Jaký je rozdíl mezi `IQueryable` a `IEnumerable`?  
6. Kdy se dotaz nad EF skutečně vykoná?  
7. Proč je vhodné používat async metody?  

---
