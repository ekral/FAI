# Policies

V následujícím kódu si ukážeme příklad na Policy určujicí zda je uživatel starší 21 let. 

Použijeme dva způsoby, v prvním zadáme  ```ClaimType``` ```Yes``` nebo ```No``` a ve druhém zadáme konkrétní věk v ```ClaimType``` ```Age```

| Id | UserId                              | ClaimType | ClaimValue |
|----|-------------------------------------|-----------|------------|
| 1  |6655f799-8baf-4b03-95ca-521b5dc9affc | AgeOver21 | Yes        |
| 2  |6655f799-8baf-4b03-95ca-521b5dc9affc | Age       | 21         |

V kódu v souboru *program.cs* potom přidáme ```Policy``` následujím způsobem s pomocí metody ```RequireClaim```:

```csharp
builder.Services.AddAuthorization(options =>
    options.AddPolicy("Over21", policy => policy.RequireClaim("AgeOver21", "Yes"))
);
```

A ```Policy`` potom použijeme obdobným způsobem jako roli:

```razor
<AuthorizeView Policy="Over21">
    <Authorized>
        <p>Hello User, @context.User.Identity?.Name! you are over 21</p>
    </Authorized>
    <NotAuthorized>
        <p>You're not over 21.</p>
    </NotAuthorized>
</AuthorizeView>
```

Pokud je logika složitejší, tak můžeme použít metodu ```RequireAssertion```:

```csharp
builder.Services.AddAuthorization(options =>
    options.AddPolicy("Over21", policy =>
        policy.RequireAssertion(context =>
        {
            Claim? claim = context.User.Claims.FirstOrDefault(claim => claim.Type == "Age");

            if (claim is null)
            {
                return false;
            }

            bool ok = int.TryParse(claim.Value, out int age);

            if (!ok)
            {
                return false;
            }

            bool result = age > 21;

            return result;
        })));
```

Použití v *razor* souboru je potom stejné jako u předchozího kódu.