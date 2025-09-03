Docker

Nazev docker instance (`--name`). Adresa musi byt stejna, nesmí se psát localhost místo 127.0.0.1.

```powershell
docker run --name keycloak -p 127.0.0.1:8080:8080 -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.3.3 start-dev
```

- Nastavit Standard flow
- Pro konzoli nastavit OAuth 2.0 Device Authorization Grant
- Pokud by jedna webova sluzba chtela volat jinou, tak povolit Client Authentication a Service account roles
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


Console configurace

- V keycloaku povolit Client authentication a Service account
