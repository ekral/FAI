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

| Name          | NormalizedName | ConcurencyStamp |
|---------------|----------------|-----------------|
| Administrator | ADMINISTRATOR  | NULL            |
| User          | USER           | NULL            |

