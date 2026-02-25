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
Student jiri = new Student() { Group = swi, Name = "Jiri" };
Student alena = new Student() { Group = swi, Name = "Alena" };

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
    public DbSet<StudentCart> Carts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne(s => s.StudentCart)
            .WithOne(sc => sc.Student)
            .HasForeignKey<StudentCart>(sc => sc.StudentId)
            .IsRequired();
    }
}
```

---

### CREATE

```csharp
Student jiri = new Student() { Name = "Jiri" };
StudentCart cart = new StudentCart() { Due = DateTime.Now.AddYears(1), Student = jiri };

context.Students.Add(jiri);
context.Carts.Add(cart);

await context.SaveChangesAsync();
```

---

## 2.3 Relace N:M (Many-to-Many)

Každá entita může být propojena s více entitami druhého typu.

### Příklad

Student může být zapsán do více kurzů a kurz může mít více studentů.

### 2.3.1 Model (implicitní spojovací tabulka)

EF Core vytvoří spojovací tabulku automaticky.

```csharp
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = "";
        public List<Course> Courses { get; set; } = new(); // Collection navigační properta
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; } = "";
        public List<Student> Students { get; set; } = new(); // Collection navigační properta
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
Student karel = new Student() { Name = "Karel" };
Subject matematika = new Subject() { Name = "Matematika" };
Subject fyzika = new Subject() { Name = "Fyzika" };

karel.Subjects = [ matematika, fyzika ];

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

# 3. Načítání souvisejících dat

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

## 3.1 Eager Loading

Používá metodu Include.

```csharp
var groups = await context.Groups
    .Include(g => g.Students)
    .ToListAsync();
```

### Výhody

- Jeden SQL dotaz
- Přehledné řešení
- Vhodné, pokud víme, že data budeme potřebovat

### Výkonové dopady

- Více JOIN operací
- Může dojít k přenosu velkého množství dat
- Riziko tzv. cartesian explosion (opakování dat při více relacích)

---

## 3.2 Explicit Loading

Relace se načte až v případě potřeby.

```csharp
var group = context.Groups.Find(1);

context.Entry(group)
    .Collection(g => g.Students)
    .Load();
```

### Výhody

- Lepší kontrola nad načítáním
- Načítáme pouze potřebná data

### Výkonové dopady

- Více SQL dotazů
- Při použití v cyklu může vzniknout větší počet dotazů

---

## 3.3 Lazy Loading

Relace se načte při prvním přístupu k navigační vlastnosti. Je nutné nakonfigurovat, ve výchozím stavu je vypnuté.

### Výkonové dopady

- Riziko tzv. N+1 problému tedy 1 dotaz na hlavní entitu + N dotazů na relace (EF Core provede 1 SQL dotaz na načtení všech kurzů. Poté pro každý kurz zvlášť provede další SQL) dotaz na studenty.
- Může výrazně zpomalit aplikaci

---

# 4. Doporučení pro praxi

Při práci s relacemi vždy přemýšlejte:

- Nejběžnější je Eeager Loading.
- Kolik SQL dotazů se provede?
- Kolik dat se skutečně načte?

---

# Kontrolní otázky

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

# 5. Závěrečný komplexní úkol – Course Management System

Navrhněte jednoduchý informační systém vysoké školy pro správu kurzů.

## Požadované entity

### Student

- Id
- FirstName
- LastName
- Courses (kolekce kurzů)
- StudentCard (relace 1:1)

### Teacher

- Id
- FirstName
- LastName
- Courses (kolekce kurzů)

### Course

- Id
- Name
- Credits
- TeacherId
- Teacher (navigační vlastnost)
- Students (kolekce studentů)

### StudentCard

- Id
- ExpirationDate
- StudentId
- Student (navigační vlastnost)

---

## Úkoly

### 1. Nastavte relace

- Student ↔ Course (N:M)
- Teacher → Course (1:N)
- Student ↔ StudentCard (1:1)

Použijte Fluent API.

---

### 2. Naplňte databázi testovacími daty

Minimálně:

- 3 students
- 2 teachers
- 3 courses
- každý student musí mít student card
- každý student musí být zapsán alespoň do jednoho kurzu

---

### 3. Implementujte dotazy

Vytvořte:

1. Výpis všech kurzů včetně učitele a počtu studentů  
2. Výpis studentů z konkrétního kurzu (včetně jejich student card)  
3. Výpis kurzů konkrétního učitele  
4. Studenty zapsané ve více než jednom kurzu  
5. Kompletní detail studenta (courses + student card)

Použijte vhodný způsob načítání souvisejících dat.

---

### 4. Ověřte chování mazání

Vyzkoušejte:

- smazání course
- smazání teacher
- smazání student

Popište, co se stane s relacemi a proč.

---
