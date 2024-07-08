# Securing WebApi

Pokud chceme zabezpečit WebApi, tak nejprve musíme přidat Identity nuget balíček:

```powershell
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

A také Entity Framework provider pro konkrétní databázi, například:

```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

