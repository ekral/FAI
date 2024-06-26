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

### Authorize atribut

V razor souboru potom můžeme použít následující atribut pro celou komponentu, který komponentu buď zpřístupní, nebo napíše text **Access denied** pokud uživatel není autorizovaný:

 ```@attribute [Authorize(Roles = "Administrator")]``` 

Můžeme také specifikovat více uživatelů:

 ```@attribute [Authorize(Roles = "Administrator, User")]``` 

## Komponenta AuthorizeView

Také můžeme použít komponentu ```AuthorizeView``` pro autorizace části komponenty.

Například:

```razor
<AuthorizeView Roles="Administrator">
    Administrator is Authorized
</AuthorizeView>
```

Také můžeme specifikovat zvlášť text pro autorizovaného uživate a jiný text pro neautorizovaného uživatele:

```razor
<AuthorizeView Roles="Administrator">
    <Authorized>
        Administrator is Authorized
    </Authorized>

    <NotAuthorized>
        Not Authorized
    </NotAuthorized>
</AuthorizeView>
```
## Informace o uživateli

Pomocí fieldu ```user``` můžeme zobrazit informace o uživateli, například uživatelské jméno:

```razor
<AuthorizeView Roles="Administrator">
    <p>Hello Administrator, @context.User.Identity?.Name!</p>
</AuthorizeView>
```

Také můžeme zobrazit všechny claimy uživatele:

```razor
<AuthorizeView Roles="Administrator">
    @if (context.User.Claims is not null)
    {
        <ul>
            @foreach (Claim claim in context.User.Claims)
            {
                <li>@claim.Type : @claim.Value</li>
            }
        </ul>
    }
</AuthorizeView>
```

A zde je ukázka výpisu:

```html
http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier : 6655f799-8baf-4b03-95ca-521b5dc9affc
http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name : ekral@utb.cz
http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress : ekral@utb.cz
AspNet.Identity.SecurityStamp : YHAHZX6EIP4VOEUWZAI72HIPRU6Y6PYL
Over21 : Yes
http://schemas.microsoft.com/ws/2008/06/identity/claims/role : Administrator
amr : pwd
```