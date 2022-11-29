Návod pro Entity Framework: [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)

Příkaz ```powershell dotnet add packgage``` stáhne nuget balíček z repozitáře nuget.org a přidá ho do projektu.

- Pokud chceme používat konkrétní databázi s Entity Frameworkem, tak musím do projektu přidat providera pro tuto databázi. Provider je většinou knihovna distriovaná jako nuget balíček. Příkaz nainstaluje nuget balíček, konrétně EF database provider pro databázi Sqlite. 

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
