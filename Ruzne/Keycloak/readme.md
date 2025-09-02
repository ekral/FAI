Docker

Nazev docker instance:

```powershell
--name nazev
```

Nastavit implicit flow.


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
