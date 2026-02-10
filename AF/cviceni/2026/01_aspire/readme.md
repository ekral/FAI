# Úkol 1 – Aspire Host

V tomto cvičení se naučíme, jak vytvořit aplikaci řízenou technologií **Aspire**.

Nejprve si stáhneme image **SQL Serveru** a spustíme jej v prostředí **Docker Desktop**. Následně vytvoříme databázi a pomocí **Service Discovery** získáme connection string.

Součástí projektu bude:

- **Aspire Application Host**, který spustí databázový server i aplikační projekty a umožní spustit HTTP command `reset-db`.
- **Minimal Web API projekt**, který bude sloužit k resetu databáze (databázi smaže, znovu vytvoří a naplní daty – vytvoří tabulku **Kniha** a vloží do ní záznamy). Projekt obsahuje také `POST` endpoint `reset-db` pro reset databáze.
- **Konzolová aplikace**, pomocí které si na konzoli zobrazíme obsah tabulky **Kniha** uložené v databázi.

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

var sql = builder.AddSqlServer("mojesql")
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = sql.AddDatabase("mojedatabase");

builder.Build().Run();
```

### 2. Class Library s entitami a DbContextem

Přidejte do solutionu nový projekt typu `Class Library` s názvem `UTB.Library.Db` a do vytvořeného projektu:
- Přidejte NuGet balíček `Aspire.Microsoft.EntityFrameworkCore.SqlServer`.
- Přidejte třídu `Author` a
- Přidejte třidu `LibraryContext`

```csharp
public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
```

```csharp
public class LibraryContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
}
```