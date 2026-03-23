Docker

Nazev docker instance (`--name`). Adresa musi byt stejna, nesmí se psát localhost místo 127.0.0.1.

```powershell
docker run --name keycloak -p 127.0.0.1:8080:8080 -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.3.3 start-dev
```

Export realmů i uživatelů. Kde `keycloack` je název kontaineru Keycloaku a `volume-name` je název volume běžící instance Keycloaku.

Nejdřív zastavíme kontainer `keycloack`, potom pustíme nový kontainer a namapujeme cestu `C:\temp\kc-export` na adresář v linuxu s exportem a připojíme existující data z původního kontajneru s názvem `volume-name` a exportujeme data do namapovaného adresáře.

```powershell
docker stop keycloak

docker run --rm -v C:\temp\kc-export:/opt/keycloak/data/export -v volume-name:/opt/keycloak/data quay.io/keycloak/keycloak:26.4 export --dir /opt/keycloak/data/export
```

Protože roles je globální scope, neupravujte ho přímo u klienta, ale v hlavním menu:

- V levém černém menu klikněte na Client scopes.
- Najděte v seznamu ten s názvem roles a klikněte na něj.
- Přejděte na záložku Mappers.
- Uvidíte tam mapper s názvem realm roles. Klikněte na něj.
- Zkontrolujte/změňte pole Token Claim Name. Pokud tam je realm_access.roles, přepište to na roles.
- Uložte (Save).


- Nastavit Standard flow
- Pokud by jedna webova sluzba chtela volat jinou, tak povolit Client Authentication a Service account roles
- Nastavit OAuth 2.0 Device Authorization Grant pokud chci generovat URL pro přihlášení.
- Nastavit Valid redirect URIs na adresu Blazoru
- Nastavit Web origins url na adresu Blazoru


1) Pridat cors.

```csharp
builder.Services.AddCors();
app.UseCors(p => p.WithOrigins("http://localhost:5014").AllowCredentials().AllowAnyHeader().AllowAnyMethod());
```

2) Přidat autentikaci

- vložit nuget
```
Microsoft.AspNetCore.Authentication.JwtBearer
```

- přidat autentikaci
```csharp
builder.Services.AddAuthentication().AddJwtBearer(c =>
{
    c.Authority = "http://127.0.0.1:8080/realms/mujrealm";
    c.Audience = "account";
    c.RequireHttpsMetadata = false;
});

app.UseAuthentication();
```

- Zabezpečit webservisu
```csharp
app.MapGet("/data", () => "joo").RequireAuthorization();
```
TODO:

Zjistit rozdil mezi standart a implicit flow

Blazor:

https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/standalone-with-authentication-library?view=aspnetcore-9.0&tabs=visual-studio

```html
<script src="_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js"></script>
```

https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-9.0#custom-authorizationmessagehandler-class



Poznámky:
- Microsoft Identity je zastřešující pojem, ale především se tím myslí Microsoft Identity Platform.
- Microsoft Identity Platform je Entra.
- [Asp.net identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio) jsou individual accounts a [identity API pro web API](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-9.0).

https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-oidc-web-authentication?view=aspnetcore-9.0

https://learn.microsoft.com/en-us/aspnet/core/blazor/security/blazor-web-app-with-oidc?view=aspnetcore-9.0&pivots=non-bff-pattern
