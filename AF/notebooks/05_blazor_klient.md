# 05 Blazor klient (SSR) nad School API

**autor: Erik Král ekral@utb.cz**

S asistencí: GitHub Copilot (GPT-5.3-Codex)

## 🎯 Definice

- Blazor je framework pro tvorbu webového UI v C#.
- SSR (Server-Side Rendering) znamená, že HTML stránky je vygenerováno na serveru.
- Render mode určuje, kde a jak se komponenta vykreslí a zda bude interaktivní.
- Aspire v tomto příkladu orchestrace spouští Web API, databázi a Blazor Web projekt společně.

Použité technologie:

- `ASP.NET Core Blazor Web App`
- `Interactive Server render mode`
- `ASP.NET Core Minimal API`
- `Entity Framework Core`
- `PostgreSQL`
- `Aspire`

---

## Co je cílem tohoto materiálu

V tomto materiálu navážeme na předchozí kapitolu o Web API a testech. Cíl je postavit Blazor klienta, který:

1. běží jako interaktivní SSR aplikace,
2. čte studenty z endpointu `GET /students` pomocí `SchoolService` a `HttpClient` a zobrazuje je v tabulce,
3. umí studenta smazat přes `DELETE /students/{id}` pomocí `SchoolService` a `HttpClient`.

Webové formuláře (Create/Edit) budeme řešit až v následujícím materiálu.

---

## 0. Jak vytvořit nový Blazor Web App projekt

Pro nový klientský projekt použijeme šablonu:

- `ASP.NET Core Blazor Web App`

Doporučené nastavení při vytváření projektu:

1. interactivity: `Interactive Server`,
2. authentication: `None` (pro toto cvičení),
3. framework: aktuální `.NET` verze podle výuky.

Pokud projekt zakládáte přes CLI, použijte šablonu Blazor Web App (`dotnet new blazor`) a následně v `Program.cs` zapněte interactive server komponenty.

---

## 1. Přehled renderovacích módů v Blazoru

Blazor Web App (od .NET 8+) podporuje více renderovacích módů:

- `Static SSR`
  - stránka se vyrenderuje na serveru jako čisté HTML,
  - po načtení není interaktivní (žádné `@onclick`).

- `Interactive Server`
  - první HTML přijde ze serveru (SSR),
  - interaktivita běží přes SignalR spojení se serverem,
  - logika komponent běží na serveru.

- `Interactive WebAssembly`
  - komponenta běží v prohlížeči přes WebAssembly,
  - po stažení runtime a assemblies je UI interaktivní na klientovi.

- `Interactive Auto`
  - kombinuje server/client přístup podle dostupnosti a konfigurace.

V tomto cvičení použijeme hlavně **Interactive Server**, protože je jednodušší a dobře se integruje s Aspire řešením.

---

## 2. Architektura řešení UTB.School

V řešení máme oddělené projekty:

- `UTB.School.WebApi` - endpointy a práce s databází
- `UTB.School.Db` - entity a `DbContext`
- `UTB.School.Contracts` - DTO objekty sdílené mezi API a klientem
- `UTB.School.Web` - Blazor klient
- `UTB.School.AppHost` - Aspire orchestrace

Webový klient nepoužívá entity z databáze, ale stabilní DTO kontrakty.

---

## 3. Aspire AppHost: propojení Web + Web API

V AppHostu je důležité, že Blazor web projekt má referenci na Web API resource:

```csharp
var webapi = builder.AddProject<Projects.UTB_School_WebApi>("webapi")
                    .WithReference(database)
                    .WaitFor(database);

_ = builder.AddProject<Projects.UTB_School_Web>("web")
           .WithReference(webapi)
           .WaitFor(webapi);
```

Tím Aspire:

- zajistí startovací pořadí,
- předá konfiguraci pro service discovery,
- umožní webu volat API přes jméno resource (`webapi`) místo pevného localhost portu.

---

## 4. Konfigurace Blazor Web projektu (SSR + Interactive Server)

V `Program.cs` projektu `UTB.School.Web` jsou klíčové dvě části.

### a) Registrace HTTP klienta pro SchoolService

V projektu máme zaregistrovaného HttpClienta jako službu s lifetimem Scoped, což znamená, že pro každý HTTP request (nebo v případě Blazoru pro každou uživatelskou session) bude vytvořena jedna instance `SchoolService` a `HttpClient`.

```csharp
builder.Services.AddHttpClient<SchoolService>(c => c.BaseAddress = new Uri("https://webapi"));
```

Proč ne `https://localhost:xxxx`?

- v Aspire se porty mohou měnit,
- název `webapi` je stabilní název resource,
- adresu doplní service discovery.

### b) Registrace a mapování render módu

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
```

První řádek registruje služby pro interactive server komponenty, druhý povolí tento render mode při mapování.

---

## 5. SchoolService

V této verzi záměrně nebudeme řešit detailní chybové zprávy. Služba bude jednoduchá: načte data nebo vraťí prázdné pole nebo false

```csharp
using System.Net;
using UTB.School.Contracts;

namespace UTB.School.Web;

public class SchoolService(HttpClient httpClient)
{
    public async Task<StudentDto[]> GetStudentsAsync()
    {
        StudentDto[]? students = await httpClient.GetFromJsonAsync<StudentDto[]>("/students");
        return students ?? [];
    }

    public async Task<bool> DeleteStudentAsync(int studentId)
    {
        HttpResponseMessage response = await httpClient.DeleteAsync($"/students/{studentId}");

        return response.StatusCode == HttpStatusCode.NoContent;
    }
}
```

Tento přístup je vhodný pro první seznámení s klientem. Robustnější error handling doplníme později.

---

## 6. Stránka Students.razor: načtení a smazání

Stránka používá `@inject SchoolService`, v `OnInitializedAsync` načte studenty a po kliknutí na tlačítko smaže vybraný záznam.

```razor
@page "/students"
@using UTB.School.Contracts
@rendermode @(new InteractiveServerRenderMode(prerender: false))
@inject SchoolService SchoolService

<PageTitle>Students</PageTitle>

<h1>Students</h1>

@if (students is null)
{
    <p><em>Loading...</em></p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>
                <th>Active</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @foreach (var student in students)
            {
                <tr>
                    <td>@student.Id</td>
                    <td>@student.Name</td>
                    <td>@student.IsActive</td>
                    <td>
                        <button class="btn btn-danger" @onclick="() => Delete(student.Id)">Delete</button>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private StudentDto[]? students;

    protected override async Task OnInitializedAsync()
    {
        await LoadStudentsAsync();
    }

    private async Task Delete(int studentId)
    {
        bool deleted = await SchoolService.DeleteStudentAsync(studentId);

        if (deleted)
        {
            await LoadStudentsAsync();
        }
    }

    private async Task LoadStudentsAsync()
    {
        students = await SchoolService.GetStudentsAsync();
    }
}
```

Poznámky k implementaci:

> 1. `InteractiveServerRenderMode(prerender: false)` znamená, že stránka nebude nejdříve staticky prerenderovaná.
> U výuky je to praktické, protože se lépe sleduje okamžik načítání dat (`Loading...`) a také se tím zabráním dvojitému načítání dat.
> 2. `@inject SchoolService SchoolService` umožňuje používat službu pro volání API, jde o dependency injection.

---

## 7. Endpointy ve Web API, které klient používá

Klient v tomto materiálu používá hlavně:

- `GET /students`
- `DELETE /students/{id:int}`

Ukázka endpointu pro smazání:

```csharp
static async Task<Results<NoContent, NotFound>> DeleteStudent(int id, SchoolContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        context.Students.Remove(student);
        await context.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
```

---

## 8. Jak aplikaci spustit

Předpoklady:

- nainstalovaný `.NET SDK`,
- spuštěný Docker Desktop (kvůli PostgreSQL kontejneru v Aspire).

Spuštění z kořene řešení:

```powershell
dotnet run --project .\UTB.School.AppHost
```

Potom:

1. přes Aspire dashboard otevřete projekt `web`,
2. otevřete stránku `/students`,
3. ověřte, že se načtou seed data,
4. smažte studenta tlačítkem `Delete` a ověřte, že zmizí z tabulky.

---

## 9. Co jsme si procvičili

- konfiguraci `HttpClient` přes Aspire service discovery,
- práci se službou `SchoolService`,
- načtení dat z API a smazání přes HTTP `DELETE`.

---

## ❓ Kontrolní otázky

1. Jaký je rozdíl mezi `Static SSR` a `Interactive Server`?
2. Proč je v Aspire výhodné použít `https://webapi` místo pevného localhost portu?
3. Kde se v Blazor komponentě nejčastěji načítají data z API?
4. Co vrací endpoint `DELETE /students/{id}` při úspěšném smazání?
5. Proč po úspěšném smazání znovu voláme načtení seznamu studentů?

---

> Poznámka: Formuláře vyřešíme v následujícím materiálu.

## Úkol

Upravte stránku `Students.razor` tak, aby šlo přepínat filtr aktivních studentů:

1. Přidejte tlačítka `All`, `Active`, `Inactive`.
2. V `SchoolService` doplňte metodu pro volání:
   - `GET /students`
   - `GET /students?isActive=true`
   - `GET /students?isActive=false`
3. Po kliknutí na filtr načtěte odpovídající data.

---

## Závěrečný úkol - Public Library (knihy)

Vytvořte obdobného Blazor SSR klienta pro téma veřejné knihovny. Cílem je použít stejný princip jako u studentů, ale nad entitou `Book`.

### API kontrakt

Použijte DTO:

- `BookDto(int Id, string Title, bool IsArchived)`

Používané endpointy:

- `GET /books`
- `GET /books?isArchived=true`
- `DELETE /books/{id}`

### Požadované části řešení

1. V Blazor projektu vytvořte službu `LibraryService`.
2. Zaregistrujte HTTP klienta přes Aspire service discovery:
    - `builder.Services.AddHttpClient<LibraryService>(c => c.BaseAddress = new Uri("https://webapi"));`
3. Vytvořte stránku `Books.razor` s route `/books`.
4. Na stránce zobrazte tabulku knih (`Id`, `Title`, `Author`, `IsArchived`).
5. Přidejte tlačítko `Delete` pro smazání knihy přes `DELETE /books/{id}`.
6. Po úspěšném smazání znovu načtěte seznam knih.

### Rozšiřující požadavky

1. Přidejte filtry `All`, `Archived`, `Not Archived`.
2. Implementujte načítání podle filtru (`/books`, `/books?isArchived=true`, `/books?isArchived=false`).
3. Ověřte, že stránka funguje v render módu `Interactive Server`.

### Odevzdání

Odevzdejte:

1. `LibraryService.cs`
2. `Books.razor`
3. stručný popis (3-5 vět), jak jste řešili refresh seznamu po smazání.
