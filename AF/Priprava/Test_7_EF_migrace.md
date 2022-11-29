Návod pro Entity Framework: [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)

Příkaz ```powershell dotnet add packgage``` stáhne nuget balíček z repozitáře nuget.org a přidá ho do projektu.

- Pokud chceme používat konkrétní databázi s Entity Frameworkem, tak musím do projektu přidat providera pro tuto databázi. Provider je většinou knihovna distriovaná jako nuget balíček. Následující příkaz nainstaluje nuget balíček, konrétně EF database provider pro databázi Sqlite. 

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

- Proto, abychom mohli vytvářet nové migrace a aktualizovat databázi, tak musíme nainstalovat nástroj **dotnet ef**. Následující příkaz nainstaluje příkaz **dotnet ef** globálně pro všechny projekty.

```powershell
dotnet tool install --global dotnet-ef
```

- Pokud chceme vytvářet migrace, tak musíme do projektu také přidat ještě následující nuget balíček.

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

Migrace představuje kód v jazyce C# který umí například vytvářet nebo aktualizovat tabulky v databázi a případně i vložit výchozí data pro model. Následující příkaz **dotnet ef** vytvoří novou migraci s názvem *VychoziMigrace*. 

```powershell
dotnet ef migrations add VychoziMigrace
```
