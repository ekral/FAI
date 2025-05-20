# 02 Entity Framework Migrace

**autor: Erik Král ekral@utb.cz**

---

Pomocí migrací můžeme vytvářet a aktualizovat databázi pomocí příkazů pro příkazovou řádku. Příkazy spouštějte v adresáři, kde se nachází projekt, tedy soubor s příponou *.csproj a před spuštěním nezapomeňte uložit všechny soubory.

## Příprava potřebných závislostí 

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

## DbContext Lifetime, Configuration, and Initialization

[odkaz](https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli)

Nástroj pro migraci musí mít k dispozici v projektu nakonfigurovaný DbContext.

1) Nakonfigurovaný jako služba v dependency injection.
2) DbContext s bezparametrickým konstruktorem nakonfigurovaný v metodě 'OnConfiguring'.
3) Pomocí design-time factory.

```csharp
public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
{
    public BloggingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");

        return new BloggingContext(optionsBuilder.Options);
    }
}
```

## Vytváření a spuštění migrací

Migrace představuje kód v jazyce C# který umí vytvářet nebo aktualizovat tabulky v databázi a případně i vložit výchozí data pro model. 

Následující příkaz **dotnet ef** vytvoří novou migraci s názvem *VychoziMigrace*.  V projektu příkaz vytvoří složku s názvem Migrations obsahující kód v jazyce C# představující migraci. Tento kód můžeme zkontrolovat a případně změnit.

```powershell
dotnet ef migrations add VychoziMigrace
```

A následující příkaz migraci aplikuje a vytvoří novou databází, nebo zaktualizuje stávající.

```powershell
dotnet ef database update
```