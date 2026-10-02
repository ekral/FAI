# 06 Blazor formuláře nad School API

**autor: Erik Král ekral@utb.cz**

S asistencí: GitHub Copilot (GPT-5.3-Codex)

## 🎯 Definice

- Blazor formulář je komponenta `EditForm`, která mapuje vstupy na C# model.
- Validace v Blazoru často používá DataAnnotations (`[Required]`, `[StringLength]`, ...).
- V tomto materiálu budeme řešit formuláře pro `Create` a `Edit` studenta.

Použité technologie:

- `Blazor Web App` (Interactive Server)
- `EditForm`, `InputText`, `InputCheckbox`
- `DataAnnotationsValidator`, `ValidationSummary`
- `SchoolService` pro volání Web API

---

## Co navazuje na minulou kapitolu

V minulé kapitole jsme měli stránku se seznamem studentů a mazání přes `DELETE`.

Teď přidáme:

1. stránku `/createstudent` pro založení záznamu,
2. stránku `/students/{id}` pro editaci,
3. validaci vstupu ve formuláři.

Stejně jako v minulé kapitole zjednodušíme ukázky a nebudeme řešit předávání detailních chybových obálek.

---

## 1. DTO a form model

Ve Web API už máme kontrakt pro vytvoření/úpravu:

```csharp
public record StudentRequestDto(string Name, bool IsActive);
```

V Blazor klientovi je praktické mít vlastní form model s validačními atributy:

```csharp
using System.ComponentModel.DataAnnotations;

namespace UTB.School.Web.FormModels;

public class StudentFormModel
{
    [Required(ErrorMessage = "Jméno je povinné.")]
    [StringLength(100, ErrorMessage = "Jméno může mít maximálně 100 znaků.")]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
```

Proč oddělovat form model od DTO:

- UI může mít vlastní validační pravidla,
- DTO zůstává jednoduchý přenosový objekt,
- form model lze později rozšířit o UI-only vlastnosti.

---

## 2. SchoolService s řádným zpracováním chyb

SchoolService používá `EnsureSuccessStatusCode()` pro automatické vyhodnocení HTTP statusů.
Pokud API vrátí chybový status, vyhodí se vyjimka `HttpRequestException`.

```csharp
using UTB.School.Contracts;

namespace UTB.School.Web;

public class SchoolService(HttpClient httpClient)
{
    public async Task<StudentDto?> GetStudentAsync(int studentId)
    {
        StudentDto? student = await httpClient.GetFromJsonAsync<StudentDto>($"/students/{studentId}");
        return student;   
    }

    public async Task CreateStudentAsync(StudentRequestDto requestDto)
    {
        var response = await httpClient.PostAsJsonAsync("/students", requestDto);
        response.EnsureSuccessStatusCode();      
    }

    public async Task UpdateStudentAsync(int studentId, StudentRequestDto requestDto)
    {
        var response = await httpClient.PutAsJsonAsync($"/students/{studentId}", requestDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteStudentAsync(int studentId)
    {
        var response = await httpClient.DeleteAsync($"/students/{studentId}");
        response.EnsureSuccessStatusCode();
    }
}
```

**Důležité:**
- `EnsureSuccessStatusCode()` vyhodí `HttpRequestException` při stavech 4xx, 5xx.
- `GetFromJsonAsync()` sám deserializuje odpověď a vrací `null` při problémech.
- Vyjimky se chytají v komponentě, nikoli zde.

---

## 3. CreateStudent.razor se zpracováním chyb

Stránka používá `EditForm` s `OnValidSubmit` a má try-catch blok pro zachycení API chyb.
Chybové zprávy se zobrazují uživateli v alert boxu.

```razor
@page "/createstudent"
@using UTB.School.Contracts
@using UTB.School.Web.FormModels
@inject NavigationManager NavigationManager
@inject SchoolService SchoolService

@if (errorMessage is not null)
{
    <div class="alert alert-danger">@errorMessage</div>
}

<h3>Create Student</h3>

@if (Model is null)
{
    <p>Loading ... </p>
}
else
{
    <EditForm Model="Model" OnValidSubmit="Submit" FormName="createStudent">
        <DataAnnotationsValidator />
        <ValidationSummary />

        <div class="mb-3">
            <label class="form-label" for="textName">Name</label>
            <InputText class="form-control" @bind-Value="Model.Name" id="textName"/>
        </div>

        <div class="mb-3 form-check">
            <InputCheckbox class="form-check-input" @bind-Value="Model.IsActive" id="checkboxIsActive" />
            <label class="form-check-label" for="checkboxIsActive">Is active</label>
        </div>
        <button class="btn btn-primary" type="submit">Submit</button>
    </EditForm>
}

@code {
    [SupplyParameterFromForm]
    public StudentFormModel? Model { get; set; }

    private string? errorMessage;

    protected override void OnInitialized()
    {
        Model ??= new() { Name = string.Empty, IsActive = true };
    }

    private async Task Submit()
    {
        if (Model is not null && Model.Name is not null)
        {
            try
            {
                errorMessage = null;

                StudentRequestDto requestDto = new(Model.Name, Model.IsActive);

                await SchoolService.CreateStudentAsync(requestDto);

                NavigationManager.NavigateTo("/students");
            }
            catch (HttpRequestException)
            {
                errorMessage = "API is not available";
            }
            catch (Polly.Timeout.TimeoutRejectedException)
            {
                errorMessage = "Timeout";
            }
        }
    }
}
```

**Klíčové body:**
- `errorMessage` je zobrazen v alert boxu, pokud je nastavena.
- Try-catch blok zachycuje `HttpRequestException` (API selhání) a timeout vyjimky.
- `Model` se inicializuje v `OnInitialized()`, aby byl vždy dostupný.
- `[SupplyParameterFromForm]` je nutný, aby se model správně naplnil z HTTP form postu v SSR/Interactive Server scénáři.
- Po úspěšném vytvoření se naviguje na `/students`.

---

## 4. EditStudent.razor se zpracováním chyb

Edit stránka má route parametr `Id`, načítá detail studenta a má error handling v obou klíčových místech:
- při načtení dat (`OnParametersSetAsync`)
- při odeslání (`Submit`)

```razor
@page "/students/{Id:int}"
@using UTB.School.Contracts
@using UTB.School.Web.FormModels
@inject NavigationManager NavigationManager
@inject SchoolService SchoolService

@if (errorMessage is not null)
{
    <div class="alert alert-danger">@errorMessage</div>
}

<h3>Edit Student</h3>

@if (Model is null)
{
    <p>Loading ... </p>
}
else
{
    <EditForm Model="Model" OnValidSubmit="Submit" FormName="editStudent">
        <DataAnnotationsValidator />
        <ValidationSummary />

        <div class="mb-3">
            <label class="form-label" for="textName">Name</label>
            <InputText class="form-control" @bind-Value="Model.Name" id="textName" />
        </div>

        <div class="mb-3 form-check">
            <InputCheckbox class="form-check-input" @bind-Value="Model.IsActive" id="checkboxIsActive" />
            <label class="form-check-label" for="checkboxIsActive">Is active</label>
        </div>

        <button class="btn btn-primary" type="submit">Save</button>
    </EditForm>
}

@code {
    [Parameter]
    public int Id { get; set; }

    [SupplyParameterFromForm]
    public StudentFormModel? Model { get; set; }

    private string? errorMessage;

    protected override async Task OnParametersSetAsync()
    {
        if (Model is not null)
        {
            return;
        }

        try
        {
            errorMessage = null;
            
            StudentDto? student = await SchoolService.GetStudentAsync(Id);

            if (student is not null)
            {
                Model = new StudentFormModel
                {
                    Name = student.Name,
                    IsActive = student.IsActive
                };
            }
            else
            {
                errorMessage = "Student was not found.";
                Model = new();
            }
        }
        catch (HttpRequestException)
        {
            errorMessage = "API is not available.";
        }
        catch (Polly.Timeout.TimeoutRejectedException)
        {
            errorMessage = "Timeout";
        }
    }

    private async Task Submit()
    {
        errorMessage = null;

        if (Model is not null && Model.Name is not null)
        {
            StudentRequestDto requestDto = new(Model.Name, Model.IsActive);

            try
            {
                errorMessage = null;
                
                await SchoolService.UpdateStudentAsync(Id, requestDto);

                NavigationManager.NavigateTo("students");
            }
            catch (HttpRequestException)
            {
                errorMessage = "API is not available";
            }
            catch (Polly.Timeout.TimeoutRejectedException)
            {
                errorMessage = "Timeout";
            }
        }
    }
}
```

**Klíčové body:**
- `OnParametersSetAsync()` se volá při nastavení parametrů `Id` a `Model`. Aby se při nastavování Model tento model znovu nepřepisoval, je zde kontrola `if (Model is not null) return;`.
- Pokud student neexistuje (vrátí `null`), zobrazí se uživateli chybová zpráva.
---

## 5. Exception handling a uživatelské chyby

V reálné aplikaci je důležité bezpečně zpracovat chyby bez úniku citlivých informací.

### HttpRequestException

Vyhodí se, když:
- je chybný HTTP status (4xx, 5xx) kvůli `EnsureSuccessStatusCode()`,
- dojde k network chybě.

```csharp
catch (HttpRequestException)
{
    errorMessage = "API is not available";
}
```

### Polly Timeout

Polly je knihovna pro resilience (retry, timeout, circuit breaker).
Pokud `HttpClient` překročí timeout, vyhodí `Polly.Timeout.TimeoutRejectedException`.

```csharp
catch (Polly.Timeout.TimeoutRejectedException)
{
    errorMessage = "Timeout";
}
```

Timeout se konfiguruje v `Program.cs` (obvykle 30 sekund):

```csharp
builder.Services.AddHttpClient<SchoolService>(c => c.BaseAddress = new Uri("https://webapi"))
    .AddTransientHttpErrorPolicy()
    .WaitAndRetryAsync(retryCount: 3, sleepDuration: TimeSpan.FromSeconds(1))
    .WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30)));
```

## 6. Co dělá [SupplyParameterFromForm]

`[SupplyParameterFromForm]` říká Blazoru, že hodnoty modelu se mají naplnit z HTTP form postu.

V našem scénáři se Static Server Side rendering je to užitečné, protože:

- model je správně navázán při submitu,
- podporuje to zpracování formuláře na serveru,
- funguje to přirozeně s `EditForm`.

Příklad:
```csharp
[SupplyParameterFromForm]
public StudentFormModel? Model { get; set; }
```

---

## 7. Validace

### Co přesně dělá `<DataAnnotationsValidator />`

- Napojí `EditForm` na validační atributy z modelu (např. `[Required]`, `[StringLength]`).
- Při změně hodnot i při odeslání formuláře vyhodnotí, zda model splňuje pravidla.
- Bez této komponenty se DataAnnotations pravidla v `EditForm` neaplikují.

### Co přesně dělá `<ValidationSummary />`

- Zobrazí souhrnný seznam validačních chyb pro celý formulář.
- Je vhodný zejména při výuce a debugování, protože student hned vidí všechny chyby na jednom místě.
- V produkčním UI se často kombinuje s `ValidationMessage` u jednotlivých polí.

Praktický příklad:

- `Name` je prázdné a má `[Required]`.
- `DataAnnotationsValidator` označí model jako nevalidní.
- `ValidationSummary` vypíše chybovou zprávu.
- `OnValidSubmit` se nespustí, takže se neodešle požadavek na API.

### Mini-ukázka: `ValidationMessage` u konkrétního pole

Kromě souhrnu lze chybu zobrazit přímo u konkrétního inputu:

```razor
<EditForm Model="Model" OnValidSubmit="Submit" FormName="createStudent">
    <DataAnnotationsValidator />

    <div class="mb-3">
        <label class="form-label" for="textName">Name</label>
        <InputText class="form-control" @bind-Value="Model.Name" id="textName" />
        <ValidationMessage For="() => Model.Name" />
    </div>

    <div class="mb-3 form-check">
        <InputCheckbox class="form-check-input" @bind-Value="Model.IsActive" id="checkboxIsActive" />
        <label class="form-check-label" for="checkboxIsActive">Is active</label>
    </div>

    <button class="btn btn-primary" type="submit">Submit</button>
</EditForm>
```

Co je výhoda:

- uživatel vidí chybu přesně u pole, které je neplatné,
- UX je přehlednější než samotný souhrn nahoře,
- dobře funguje v kombinaci `ValidationSummary + ValidationMessage`.

---

## 8. Jak si to vyzkoušet

1. Spusťte AppHost:

```powershell
dotnet run --project .\UTB.School.AppHost
```

2. Otevřete Blazor web aplikaci.
3. Na stránce `/students` klikněte na `Create`.
4. Vytvořte nového studenta.
5. U záznamu klikněte na `Edit`, upravte data a uložte.

Při testu error handlingu:
- Vypněte API a zkuste vytvořit studenta — zobrazí se "Timeout".
- Vytvořte studenta s prázdným jménem — formulář nebude odeslán kvůli validaci.

---

## 9. Shrnutí

V této kapitole jsme doplnili Blazor klienta o formuláře:

- vytvoření studenta (`POST /students`),
- editaci studenta (`PUT /students/{id}`),
- validační pravidla na úrovni form modelu,
- přesměrování po úspěšném uložení zpět na seznam.

---

## 10. Kontrolní otázky

1. Jaký je rozdíl mezi `OnSubmit` a `OnValidSubmit`?
2. Proč je vhodné mít `StudentFormModel` odděleně od `StudentRequestDto`?
3. K čemu slouží `[SupplyParameterFromForm]`?
4. Jak funguje `EnsureSuccessStatusCode()`?
5. Jaké vyjimky chytáme v komponentě a proč je ošetřujeme ve `SchoolService`?
6. Proč po vytvoření nebo úpravě záznamu navigujeme zpět na `/students`?
7. Jaký je rozdíl mezi `HttpRequestException` a `Polly.Timeout.TimeoutRejectedException`?

---

## Zdroje

1. [ASP.NET Core Blazor forms overview](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/?view=aspnetcore-9.0)
2. [ASP.NET Core Blazor input components](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/input-components?view=aspnetcore-9.0)