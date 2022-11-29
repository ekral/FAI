Návod pro Entity Framework: [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)

Příkaz ```powershell dotnet add packgage``` stáhne nuget balíček z repozitáře nuget.org a přidá ho do projektu.

- Příkaz nainstaluje nuget balíček EF database provider, konrétně pro databázi Sqlite.

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

- Proto, abychom mohli vytvářet nové migrace a aktualizovat databázi, tak musíme nainstalovat nástroj **dotnet ef**. Následující příkaz nainstaluje **dotnet ef** globálně pro všechny projekty.

```powershell
dotnet tool install --global dotnet-ef
```

- Příkaz vygeneruje C# program pro vytvoření tabulek v databázi a případně i výchozích dat pro model.

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```
