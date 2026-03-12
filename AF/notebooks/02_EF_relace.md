# 02 – Relace v Entity Framework Core

**autor: Erik Král ekral@utb.cz**

---

## 🎯 Cíle kapitoly

Po prostudování této kapitoly budete schopni:

- vysvětlit rozdíl mezi relací 1:1, 1:N a N:M
- vysvětlit význam cizího klíče
- rozlišit navigation property a jejich roli
- nastavit relace pomocí Fluent API
- vysvětlit rozdíl mezi Eager, Explicit a Lazy loadingem
- popsat výkonové dopady jednotlivých způsobů načítání dat
- navrhnout vlastní datový model s relacemi

---

# 1. Co je relace?

V relační databázi jsou data rozdělena do tabulek.  
**Relace** určují, jak spolu tabulky souvisejí.

Například:

- Student patří do jedné skupiny  
- Skupina obsahuje více studentů  

Relace je v databázi realizována pomocí **cizího klíče (foreign key)**.

V EF Core se relace skládá ze dvou částí:

- cizí klíč
- Navigační properta (v C# třídách)

Entity Framework Core využívá konvence, díky kterým dokáže automaticky rozpoznat relace bez nutnosti explicitní konfigurace.

EF Core typicky odvodí relaci, pokud:

- Existuje navigační vlastnost (např. `public Student Student { get; set; }`)

- Existuje odpovídající cizí klíč pojmenovaný podle konvence (například `StudentId`).

Například:

```csharp
public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }   // EF rozpozná jako FK
    public Student? Student { get; set; } // navigační property, je nullable protože EF ji defaultně nenačítá
}
```

Pokud jsou dodrženy konvence, není nutné používat Fluent API ani atributy.
Konfigurace je potřeba až v případě nestandardního pojmenování nebo složitější relace.

---

# 2. Typy relací

## 2.1 Relace 1:N (One-to-Many)

Jedna entita může souviset s více entitami druhého typu.

### Příklad

Jedna skupina má více studentů, ale student patří pouze do jedné skupiny.

### Model

```csharp
public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int GroupId { get; set; }        // cizí klíč
    public Group? Group { get; set; }       // Navigation Property
}

public class Group
{
    public int Id { get; set; }
    public required string Title { get; set; }

    public List<Student> Students { get; set; } = []; // Collection Navigation Property
}
```
---

### Fluent API konfigurace a DbContext

- V tomto případě to tedy **není nutné**, ale pro větší názornost si ukažeme jak bychom nakonfigurovali relace pomocí fluent API a zároveň si ukážeme jak by vypadal `DbContext`.

```csharp
using Microsoft.EntityFrameworkCore;

public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Group { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
                    .HasOne(s => s.Group)
                    .WithMany(sk => sk.Students)
                    .HasForeignKey(s => s.GroupId);
    }
}
```

---

### CREATE

Pro zadávání relací můžeme použít jak cizí klíč, ale také navigační property jak je ukázané v příkladě níže.

```csharp
Skupina swi = new Skupina() { Tittle = "SWI1" };
Student jiri = new Student() { Name = "Jiri", Group = swi };
Student alena = new Student() { Name = "Alena", Group = swi };

context.Skupiny.Add(swi);
context.Studenti.AddRange(jiri, alena);

context.SaveChanges();
```

--- 

## 2.2 Relace 1:1 (One-to-One)

Každá entita má právě jednu odpovídající entitu. Rozlišujeme principal entity a dependent entity. Obě mají navigační property, ale dependent entity má navíc cizí klíč.

### Příklad

Student (principal entity) má jednu studentskou kartu (dependent entity).

### Model

```csharp
public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public StudentCard? StudentCard { get; set; }   // navigační properta
}

public class StudentCard
{
    public int Id { get; set; }
    public DateTime Due { get; set; }

    public int StudentId { get; set; }      // cizí klíč
    public Student? Student { get; set; }   // navigační properta
}
```
---

### Fluent API konfigurace a DbContext

- EF nastaví kód podle jmenných konvencí, niže je příklad jak nastavit relace pomocí fluent API.

```csharp
class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCard> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne(s => s.StudentCard)
            .WithOne(sc => sc.Student)
            .HasForeignKey<StudentCard>(sc => sc.StudentId)
            .IsRequired();
    }
}
```

---

### CREATE

```csharp
Student jiri = new Student() { Name = "Jiri" };
StudentCard card = new StudentCard() { Due = DateTime.Now.AddYears(1), Student = jiri };

context.Students.Add(jiri);
context.Cards.Add(card);

await context.SaveChangesAsync();
```

---

## 2.3 Relace N:M (Many-to-Many)

Každá entita může být propojena s více entitami druhého typu.

### Příklad

Student může být zapsán do více kurzů a kurz může mít více studentů.

### 2.3.1 Model (implicitní spojovací tabulka)

EF Core automaticky vytvoří spojovací tabulku v databázi, která obsahuje:
- cizí klíč na entitu Student
- cizí klíč na entitu Course
- složený primární klíč z obou těchto sloupců

Tato tabulka nemá vlastní entitu v modelu – existuje pouze v databázi. Pokud potřebujeme do relace přidat další atributy (např. datum zápisu), musíme vytvořit explicitní spojovací entitu. Tabulka je pojmenována kombinací názvů entit (např. StudentCourse).

```csharp
    public class Student
    {
        public int StudentId { get; set; }
        public required string Name { get; set; }
        public List<Course> Courses { get; set; } = []; // Collection navigační properta
    }

    public class Course
    {
        public int CourseId { get; set; }
        public required string Name { get; set; }
        public List<Student> Students { get; set; } = []; // Collection navigační properta
    }
```

---

#### Fluent API konfigurace a DbContext

Vše se nakonfiguruje s pomocí jmenných konvencí. V tomto případě to tedy není nutné, ale pro větší názornost si opět ukažeme jak bychom nakonfigurovali relace pomocí fluent API:


```csharp
class SchoolContext(DbContextOptions<SchoolContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
                    .HasMany(student => student.Subjects)
                    .WithMany(subject => subject.Students);
                
    }
}
```

---

#### CREATE

```csharp
Subject matematika = new Subject() { Name = "Matematika" };
Subject fyzika = new Subject() { Name = "Fyzika" };

Student karel = new Student() { Name = "Karel", Subjects =  [ matematika, fyzika ] };

await context.AddAsync(karel);

int count = context.SaveChanges();
```

---

### 2.3.2 Model (Many-to-many with class for join entity)

U varianty [many-to-many with class for join entity](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-with-class-for-join-entity) si přímo nadefinujeme propojovací třídu `StudentSubject` a označíme ji pomocí Fluent API. Výhodou je, že můžeme snadněji zadávat její hodnoty.

Budeme mít tedy následující třídy, kdy proti předchozímu příkladu přibyla třída `StudentSubject`.

```csharp
class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Subject> Subjects { get; set; } = [];
}

class Subject
{
    public int Id { get; set; }
    public required string Tittle { get; set; }
    public List<Student> Students { get; set; } = [];
}

class StudentSubject
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
}
```

---

#### Fluent API konfigurace a DbContext

```csharp
class SchoolContext(DbContextOptions<SchoolContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasMany(student => student.Subjects)
            .WithMany(skupina => skupina.Students)
            .UsingEntity<StudentSubject>();
    }
}
```

---

# 3. Práce s daty a tvorba dotazů nad relacemi

## 3.1 Dotazy nad relacemi

V relační databázi jsou data rozdělena do více tabulek a propojena pomocí relací (např. Student – Zápis – Předmět). Entity Framework umožňuje nad těmito relacemi vytvářet dotazy pomocí LINQ.

Důležité je si uvědomit, že dotazy nad DbSet se překládají do SQL a provádějí přímo v databázi. Data tedy není nutné nejprve načíst do paměti a až poté filtrovat.

Předpokládejme následující model:

- `Student`
- `Course`
- `Enrollment` (zápis studenta na předmět)

```csharp
public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; } // cizí klíč na studenta
    public Student Student { get; set; } // navigační vlastnost

    public int CourseId { get; set; } // cizí klíč na předmět
    public Course Course { get; set; } // navigační vlastnost

    public DateOnly Date { get; set; }
    public int? Grade { get; set; }
}
```

Relace mezi entitami:

`Student` 1 --- * `Enrollment` * --- 1 `Course`

Student může být zapsán na více předmětů a každý předmět může mít více studentů.

---

### Filtrování podle toho zda existuje zápis na předmět

Častým požadavkem je výběr entit podle dat v jiné tabulce.

Například chceme získat všechny studenty, kteří jsou zapsáni alespoň na jeden předmět.

Dotaz využívá relaci: 

`Student` → `Enrollment`.

```csharp
var students = context.Students
.Where(s => s.Enrollments.Any());
```

Metoda Any() testuje, zda existuje alespoň jeden související záznam.

---

### Filtrování podle cizího klíče

Můžeme také filtrovat podle `Id` předmětu pomocí cizího klíče `Enrollment.CourseId`.

Dotaz využívá relaci:

`Student` → `Enrollment`.

```csharp
var students = context.Students
.Where(s => s.Enrollments.Any(e => e.CourseId == 1));
```

Dotaz vrátí všechny studenty, kteří jsou zapsáni na předmět s daným identifikátorem.

---

### Filtrování podle zápisu na konkrétní předmět

Chceme najít studenty zapsané na předmět s názvem „Databáze“. Tentokrát už probíhá dotaz nad třemi tabulkami, protože přistupujeme pomocí navigační property `Enrollement.Course` k názvu předmětu.

Dotaz využívá relaci:

`Student` → `Enrollment` → `Course`.

```csharp
var students = context.Students
.Where(s => s.Enrollments.Any(e => e.Course.Name == "Databáze"));
```

Entity Framework tento LINQ dotaz přeloží na SQL dotaz s odpovídajícími JOIN.

---

### Projekce dat z více tabulek

Pokud chceme vrátit kombinovaná data (např. jméno studenta a název předmětu), použijeme Select:

```csharp
var result = context.Enrollments
    .Select(e => new 
    {
        Student = e.Student.LastName,
        Course = e.Course.Name
    })
```

Tento dotaz vrátí seznam dvojic student–předmět.

---

## 3.2 Načítání souvisejících dat

Definice relace neznamená, že se související data načtou automaticky. Sami musíme určit způsob načítání.

Pokud bychom tedy načetli skupinu následujícím způsobem, tak by navigation property `skupina.Studenti` měla nulový počet studentů:

```csharp
var groups = context.Groups.ToList();

foreach(Group group in groups)
{
    Console.WriteLine($"{group.Title} Pocet studentu: {group.Students.Count}");
}
```

EF Core nabízí tři přístupy jak načítat data:

- Eager loading
- Explicit loading
- Lazy loading

---

### 3.2.1 Eager Loading

Používá metodu Include.

```csharp
var groups = await context.Groups
    .Include(g => g.Students)
    .ToListAsync();
```

#### Výhody

- Jeden SQL dotaz
- Přehledné řešení
- Vhodné, pokud víme, že data budeme potřebovat

#### Výkonové dopady

- Více JOIN operací
- Může dojít k přenosu velkého množství dat
- Riziko tzv. cartesian explosion (opakování dat při více relacích)

---

### 3.2.2 Explicit Loading

Relace se načte až v případě potřeby.

```csharp
var group = context.Groups.Find(1);

context.Entry(group)
    .Collection(g => g.Students)
    .Load();
```

#### Výhody

- Lepší kontrola nad načítáním
- Načítáme pouze potřebná data

#### Výkonové dopady

- Více SQL dotazů
- Při použití v cyklu může vzniknout větší počet dotazů

---

### 3.2.3 Lazy Loading

Lazy loading znamená, že související data nejsou načtena z databáze společně s hlavní entitou, ale až ve chvíli, kdy k nim aplikace skutečně přistoupí prostřednictvím navigační property.

Například při načtení studenta se nenačtou jeho kurzy. Ty se načtou až ve chvíli, kdy k nim program poprvé přistoupí.

#### Jak funguje

EF Core při zapnutém lazy loadingu vytvoří tzv. proxy objekty. Ty zachytí přístup k navigační vlastnosti a automaticky odešlou dodatečný SQL dotaz do databáze. 

Lazy loading není ve výchozím stavu zapnutý, pro správnou funkci je nutné jej explicitně povolit:
- nainstalovat balíček `Microsoft.EntityFrameworkCore.Proxies`
- aktivovat lazy loading v konfiguraci `DbContextu` pomocí `options.UseLazyLoadingProxies();`
- označit navigační property jako `virtual`

#### Výkonové dopady

- Riziko tzv. N+1 problému tedy 1 dotaz na hlavní entitu + N dotazů na relace (EF Core provede 1 SQL dotaz na načtení všech kurzů. Poté pro každý kurz zvlášť provede další SQL) dotaz na studenty.
- Může výrazně zpomalit aplikaci

---

### Doporučení pro praxi

Při práci s relacemi vždy přemýšlejte:

- Nejběžnější je Eeger Loading.
- Kolik SQL dotazů se provede?
- Kolik dat se skutečně načte?

---

# 🧩 4. Závěrečný komplexní úkol – Library Management System

Navrhněte jednoduchý informační systém veřejné knihovny pomocí Entity Framework Core.

## 📌 Požadované entity

Implementujte následující třídy:

- Book (kniha)
    - Id  
    - Title  
- Author (autor)
    - Id  
    - Name  
- Reader (čtenář)
    - Id  
    - Name  
- Loan (výpůjčka)
    - Id  
    - LoanDate  
    - ReturnDate (nullable)  
  
---

### 🔗 Požadované relace

Entity budou mít následující relace, doplňte cizí klíče a navigační property:

- Book ↔ Author (vztah M:N mezi knihou a autorem)  
- Reader → Loan (vztah 1:N mezi čtenářem a výpůjčkou)  
- Book → Loan (vztah 1:N mezi knihou a výpůjčkou)  

### 📊 Testovací data

Vytvořte a uložte do databáze:

- alespoň 3 knihy
- alespoň 3 autory 
- alespoň 2 čtenáře
- alespoň 2 výpůjčky

> Zamyslete se nad vztahem ReaderCard (čtenářský průkaz) a Reader a implementujte jej.
---

### 🔍 Implementujte LINQ dotazy

1. Vypište všechny knihy včetně jejich autorů.  
2. Vypište názvy všech knih vypůjčených (vrácených i nevrácených) konkrétním čtenářem.  
3. Najděte a vypište čtenáře s více než jednou aktivní výpůjčkou (ReturnDate je null).  
4. Vypište knihy, které nikdy nebyly půjčeny.  

### 🧪 Ověřte referenční integritu

Otestujte chování aplikace při smazání:

- autora  
- knihy  
- čtenáře 

Popište výsledek.

---

# ❓ Kontrolní otázky

Odpovězte na následující otázky bez použití materiálů. Pokud si nejste jistí, vraťte se k příslušné části kapitoly.

1. Jaký je rozdíl mezi relací 1:1, 1:N a N:M?
2. Co je cizí klíč a jakou roli plní v databázi?
3. Jaký je rozdíl mezi cizím klíčem a navigační vlastností?
4. Jak se nastavuje relace pomocí Fluent API?
5. Kdy EF Core vytvoří spojovací tabulku automaticky?
6. Jaký je rozdíl mezi Eager, Explicit a Lazy loadingem?
7. Proč může Eager loading způsobit přenos velkého množství dat?
8. V jaké situaci byste použili Explicit loading?
9. Co se může stát při smazání entity, která je navázána na jiné entity?

---
