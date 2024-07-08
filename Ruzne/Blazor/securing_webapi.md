# Securing WebApi

Pokud chceme zabezpečit WebApi, tak nejprve musíme přidat Identity nuget balíček:

```powershell
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

A také Entity Framework provider pro konkrétní databázi, například:

```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Dále vytvoříme DbContext:

```csharp
```

---
1. [How to use Identity to secure a Web API backend for SPAs](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-8.0)