# 01 – Základy Entity Framework Core

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
    public string Name { get; set; }
    public int Age { get; set; }
}
```

- `Id` je primární klíč  
- instance odpovídá řádku v tabulce  
- EF sleduje změny objektů  

---

# 5. DbContext

```csharp
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=students.db");
    }
}
```

`DbSet<Student>` reprezentuje tabulku a umožňuje psát LINQ dotazy.

---

# 6. Vytvoření databáze

```csharp
using var context = new StudentContext();
context.Database.EnsureCreated();
```

`EnsureCreated()` není vhodné pro produkci.

---

# 7. CRUD operace

## CREATE

```csharp
var student = new Student
{
    Name = "Jan",
    Age = 22
};

context.Students.Add(student);
context.SaveChanges();
```

## READ

```csharp
var students = context.Students.ToList();

var older = context.Students
                   .Where(s => s.Age > 20)
                   .ToList();

var student = context.Students.Find(1);

var student = context.Students
                     .FirstOrDefault(s => s.Name == "Jan");
```

## UPDATE

```csharp
var student = context.Students.Find(1);
student.Age = 23;
context.SaveChanges();
```

## DELETE

```csharp
var student = context.Students.Find(1);
context.Students.Remove(student);
context.SaveChanges();
```

---

# 8. Asynchronní přístup

V reálném kódu používám většinou asynchronní varianty, například:

```csharp
var students = await context.Students.ToListAsync();
await context.SaveChangesAsync();
```

---

# 9. Překlad LINQ do SQL

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

# 12. Kontrolní otázky

1. Co je ORM?  
2. Jaký je rozdíl mezi LINQ to Objects a LINQ to Entities?  
3. Co je extension metoda?  
4. Co znamená odložené vykonání?  
5. Jaký je rozdíl mezi `IQueryable` a `IEnumerable`?  
6. Kdy se dotaz nad EF skutečně vykoná?  
7. Proč je vhodné používat async metody?  

---