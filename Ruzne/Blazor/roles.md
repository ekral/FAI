# Role v Blazor SSR

Pro zprovoznění rolí musíme přidat kód ```AddRoles<IdentityRole>()``` do konfigurace IdentityCore:

```csharp
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
```

Dále do tabulky **AspNetRoles** přidáme role, například:

| Id | Name         | NormalizedName | ConcurencyStamp |
|----|--------------|----------------|-----------------|
| 1  |Administrator | ADMINISTRATOR  | NULL            |
| 2  |User          | USER           | NULL            |


A přiřadíme uživateli role s pomocí tabulky **AspNetUserRoles**, například:

| UserId                               | RoleId | 
|--------------------------------------|--------|
| 6655f799-8baf-4b03-95ca-521b5dc9affc | 1      |

V razor souboru potom můžeme použít atribut ```@attribute [Authorize(Roles = "Administrator")]``` pro celou komponentu, který komponentu buď zpřístupní, nebo napíše text **Access denied** pokud uživatel není autorizovaný.