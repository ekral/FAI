# Úkol 1 – Aspire Host

V tomto cvičení se naučíme, jak vytvořit aplikaci řízenou technologií **Aspire**.

Nejprve si stáhneme image **SQL Serveru** a spustíme jej v prostředí **Docker Desktop**. Následně vytvoříme databázi a pomocí **Service Discovery** získáme connection string.

Součástí projektu bude:

- **Aspire Application Host**, který spustí databázový server i aplikační projekty a umožní spustit HTTP command `reset-db` dostupný v Aspire Dashboardu.
- **Minimal Web API projekt**, který bude sloužit k resetu databáze (databázi smaže, znovu vytvoří a naplní daty – vytvoří tabulku **Kniha** a vloží do ní záznamy). Projekt obsahuje také `POST` endpoint `reset-db` pro reset databáze.

## 📋 Postup

### 1. Aspire Application Host 

Vytvořte nový projekt typu **Aspire Empty App** s názvem `UTB.Library` a pro vytvořený projekt:
- Zaktualizujte případné zastaralé NuGet balíčky.
- Přidejte NuGet balíček `Aspire.Hosting.SqlServer` (viz [návod pro SQL Server](https://aspire.dev/integrations/databases/efcore/sql-server/sql-server-get-started/)).
- Přidejte do kódu vytvoření SQL Serveru a databáze (viz kód níže).
- Spusťte Docker Desktop nebo Podman (pro Podman je nutné [nastavit Environment Variable](https://aspire.dev/get-started/prerequisites/#install-an-oci-compliant-container-runtime)).
- Spusťte aplikaci a počkejte, než se stáhne Docker image a spustí se server a databáze. Prozkoumejte Aspire Dashboard.

Metoda `WithDataVolume` přidá ukládání dat na disk. Změny, které provedeme v SQL Serveru, se tedy po vypnutí kontejneru uloží a po dalším spuštění znovu použijí.  
Volba `ContainerLifetime.Persistent` znamená, že při vypnutí aplikace zůstane kontejner běžet.

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = sql.AddDatabase("database");

builder.Build().Run();
```

### 2. Class Library s entitami a DbContextem

Přidejte do solutionu nový projekt typu `Class Library` s názvem `UTB.Library.Db` a do vytvořeného projektu:
- Přidejte NuGet balíček `Aspire.Microsoft.EntityFrameworkCore.SqlServer`.
- Přidejte třídu `Author`.
- Přidejte třídu `LibraryContext` (doplňte chybějící `using Microsoft.EntityFrameworkCore;` pomocí QuickActions).

```csharp
public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
```

```csharp
public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
}
```

### 3. DatabaseManager pro reset databáze

Přidejte do solution nový projekt typu `ASP.NET Core Web API` s názvem `UTB.Library.DbManager` s nastavením:
- AuthenticationType: none
- Configure for HTTPS: yes
- Enlist in .NET Aspire orchestration: yes
- Ostatní volby nejsou vybrány.

Do projektu potom přidejte:
- Referenci na projekt `UTB.Library.Db`.
- Kód mapující POST metodu na routu `/reset-db`. POST endpoint mění obsah databáze pomocí Entity Frameworku. Řetězec "database" v kódu odkazuje na connection string do databáze definovaný v Application Hostu a použitý pomocí Service Discovery.

```csharp
using UTB.Library.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<LibraryContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/reset-db", async (LibraryContext context) =>
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    Author a1 = new() { Name = "Karel Capek" };
    Author a2 = new() { Name = "Jaroslav Hasek" };
    Author a3 = new() { Name = "Bohumil Hrabal" };

    context.Authors.AddRange(a1, a2, a3);

    await context.SaveChangesAsync();
});

app.UseHttpsRedirection();

app.Run();
```

### 4. HTTP Command v Aspire Dashboardu

V projektu `UTB.Library.AppHost`
- Přidejte do souboru `AppHost.cs` referenci na databázi a čekání na její dostupnost.
- A do stejného souboru přidejte HTTP Command, pomocí kterého bude možné z Aspire Dashboardu spustit reset databáze.

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = sql.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("utb-library-dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

builder.Build().Run();
```

## ✅ Výsledek

Po dokončení úkolu:
- běží SQL Server v Dockeru řízený Aspire,
- databáze je dostupná přes Service Discovery,
- HTTP Command `reset-db` resetuje a seeduje databázi,
- Aspire Dashboard umožňuje řízení celé aplikace.