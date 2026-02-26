# Úkol 01: Aspire Host

*Autor: Erik Král – <ekral@utb.cz>*

V tomto cvičení se naučíme, jak vytvořit aplikaci řízenou technologií **Aspire** využívající Docker kontejnery.

Instalace všech nástrojů (databázové servery, nástroje pro distribuovanou cache (např. Redis), nástroje pro zasílání zpráv atd.) pro každý projekt by byla velmi náročná. Zároveň je vhodné s nástroji pracovat ve stejném prostředí (včetně operačního systému), ve kterém budou nasazeny. Proto využíváme virtualizaci pomocí kontejnerů — stáhneme si image s operačním systémem a například databázovým serverem, můžeme s ním pracovat a poté jej jednoduše odstranit.

Spouštění různých serverů, webových služeb a klientů musí probíhat v určitém pořadí a jednotlivé komponenty musí být vzájemně propojeny pomocí connection stringů k databázím a adres serverů hostujících webové a další služby. K tomuto účelu slouží Aspire, který řídí (orchestruje) spouštění distribuovaných aplikací a lze jej také použít pro jejich nasazení.

Nejprve si stáhneme image databáze **ProgreSQL** a spustíme jej v prostředí **Docker Desktop**. Následně vytvoříme databázi a pomocí **Service Discovery** získáme connection string.

Součástí projektu bude:

- **Aspire Application Host**, který spustí databázový server i aplikační projekty a umožní spustit HTTP command `reset-db` dostupný v Aspire Dashboardu.
- **Minimal Web API projekt**, který bude sloužit k resetu databáze (databázi smaže, znovu vytvoří a naplní daty – vytvoří tabulku **Kniha** a vloží do ní záznamy). Projekt obsahuje také `POST` endpoint `reset-db` pro reset databáze.

---

## 📋 Postup

U všech projektů zvolte **.NET 10**.

### 1. Aspire Application Host 

Vytvořte nový projekt typu **Aspire Empty App** (Prázdná aplikace Aspire) s názvem `UTB.Library` a pro vytvořený projekt:
- Zaktualizujte případné zastaralé NuGet balíčky.
- Přidejte NuGet balíček `Aspire.Hosting.PostgreSQL` (viz [návod pro PostgreSQL](https://aspire.dev/integrations/databases/postgres/postgres-get-started/?lang=csharp)).
- Přidejte do souboru `AppHost.cs` vytvoření PostgreSQL Serveru a databáze (viz kód níže).
- Spusťte Docker Desktop nebo Podman (pro Podman je nutné [nastavit Environment Variable](https://aspire.dev/get-started/prerequisites/#install-an-oci-compliant-container-runtime)).
- Spusťte aplikaci a počkejte, než se stáhne Docker image a spustí se server a databáze. Prozkoumejte Aspire Dashboard.

Metoda `WithDataVolume` přidá ukládání dat na disk. Změny, které provedeme v PostgreSQL Serveru, se tedy po vypnutí kontejneru uloží a po dalším spuštění znovu použijí.  
Volba `ContainerLifetime.Persistent` znamená, že při vypnutí aplikace zůstane kontejner běžet.
Metoda WithPgAdmin přidá do projektu PgAdmina pro administraci PostgreSQL serveru.

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent))
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database");

builder.Build().Run();
```

--- 

### 2. Class Library s entitami a DbContextem

Přidejte do solutionu nový projekt typu `Class Library` (knihovna tříd) s názvem `UTB.Library.Db` a do vytvořeného projektu:
- Přidejte NuGet balíček `Aspire.Npgsql.EntityFrameworkCore.PostgreSQL`.
- Přidejte třídu `Author`.
- Přidejte třídu `LibraryContext` (doplňte chybějící `using Microsoft.EntityFrameworkCore;` pomocí QuickActions).

```csharp
using Microsoft.EntityFrameworkCore;

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

---

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

builder.AddNpgsqlDbContext<LibraryContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/reset-db", async (LibraryContext context) =>
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    Author capek = new() { Name = "Karel Capek" };
    Author hasek = new() { Name = "Jaroslav Hasek" };
    Author hrabal = new() { Name = "Bohumil Hrabal" };

    context.Authors.AddRange(capek, hasek, hrabal);

    await context.SaveChangesAsync();
});

app.UseHttpsRedirection();

app.Run();
```

---

### 4. HTTP Command v Aspire Dashboardu

V projektu `UTB.Library.AppHost`
- Přidejte do souboru `AppHost.cs` referenci na databázi a čekání na její dostupnost.
- A do stejného souboru přidejte HTTP Command, pomocí kterého bude možné z Aspire Dashboardu spustit reset databáze.

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                 .WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent))
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

builder.Build().Run();
```

---

## ✅ Výsledek

Po dokončení úkolu:
- běží PostgreSQL Server v Dockeru řízený Aspire,
- běží a je nakonfigurovaný PgAdmin,
- databáze je dostupná přes Service Discovery,
- HTTP Command `reset-db` resetuje a seeduje databázi,
- Aspire Dashboard umožňuje řízení celé aplikace.

---

## Samostatný úkol

Vytvořte Solution a projekty znovu, ale tentokrát s využitím Microsoft SQL databáze [SQL Server](https://aspire.dev/integrations/databases/efcore/sql-server/sql-server-get-started/) a porovnejte, která databáze zabírá méně prostředků.
