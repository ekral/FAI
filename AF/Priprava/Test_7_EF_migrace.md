# Entity framework

Návod pro Entity Framework: [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)

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
class Student
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

class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }
}
```

## Migrace



Pomocí migrací můžeme vytvářet a aktualizovat databázi pomocí příkazů pro příkazovou řádku.

### Příprava potřebných závislostí 

Proto, abychom mohli vytvářet nové migrace a aktualizovat databázi, tak musíme nainstalovat:
- nástroj **dotnet ef**. 
- Nuget balíček podporující vytváření migrací **Microsoft.EntityFrameworkCore.Design**

Následující příkaz nainstaluje příkaz **dotnet ef** globálně pro všechny projekty.

```powershell
dotnet tool install --global dotnet-ef
```

A následující příkaz přída do projektu nuget balíček **Microsoft.EntityFrameworkCore.Design**.

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

---
## Úkol na cvičení

1) Nadefinujte třídu Pizza z projektu [Pizza.Kiosk](https://github.com/ekral/Utb.PizzaKiosk) a pomoci Entity Frameworku a migrací vytvořte Sqlite databázi. Do databáze vložte tři pizzy jako výchozí data při vytvoření databáze.
   - Definujte třídu Pizza.
   - Definujte potomka třídy DbContext a DbSet pro tabulku Pizza.
   - Definujte metodu OnConfiguring a nakonfigurujte Sqlite.
   - Definujte metodu OnCreating a přidejte výchozí pizzy.
   - Vytvořte a proveďte migraci (vytvořte databázi).
3) Vytvořte kód který vypíše všechny pizzy z databáze.
4) Vytvořte kód, který vypíše pouze pizzy levnější než 150.
5) Vytvořte kód, který vloží do databáze novou pizzu.
