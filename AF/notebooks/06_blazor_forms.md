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

## 2. Jednoduchý SchoolService pro formuláře

Pro potřeby create/edit nám stačí tři metody:

```csharp
using System.Net;
using UTB.School.Contracts;

namespace UTB.School.Web;

public class SchoolService(HttpClient httpClient)
{
    public async Task<StudentDto?> GetStudentAsync(int studentId)
    {
        var response = await httpClient.GetAsync($"/students/{studentId}");

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<StudentDto>();
    }

    public async Task<bool> CreateStudentAsync(StudentRequestDto requestDto)
    {
        var response = await httpClient.PostAsJsonAsync("/students", requestDto);
        return response.StatusCode == HttpStatusCode.Created;
    }

    public async Task<bool> UpdateStudentAsync(int studentId, StudentRequestDto requestDto)
    {
        var response = await httpClient.PutAsJsonAsync($"/students/{studentId}", requestDto);
        return response.StatusCode == HttpStatusCode.NoContent;
    }
}
```

Kód je zjednodušený. V produkci budeme chtít lepší diagnostiku chyb, ale pro výuku formulářů je tento tvar přehlednější.

---

## 3. CreateStudent.razor

Stránka používá `EditForm` s `OnValidSubmit`. Po úspěšném odeslání přesměruje uživatele na seznam studentů.

```razor
@page "/createstudent"
@using UTB.School.Contracts
@using UTB.School.Web.FormModels
@inject NavigationManager NavigationManager
@inject SchoolService SchoolService

<h3>Create Student</h3>

<EditForm Model="Model" OnValidSubmit="Submit" FormName="createStudent">
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

    <button class="btn btn-primary" type="submit">Submit</button>
</EditForm>

@code {
    [SupplyParameterFromForm]
    public StudentFormModel Model { get; set; } = new();

    private async Task Submit()
    {
        StudentRequestDto requestDto = new(Model.Name, Model.IsActive);
        bool created = await SchoolService.CreateStudentAsync(requestDto);

        if (created)
        {
            NavigationManager.NavigateTo("/students");
        }
    }
}
```

---

## 4. EditStudent.razor

Edit stránka má route parametr `Id`, při načtení si stáhne detail studenta a předvyplní formulář.

```razor
@page "/students/{Id:int}"
@using UTB.School.Contracts
@using UTB.School.Web.FormModels
@inject NavigationManager NavigationManager
@inject SchoolService SchoolService

<h3>Edit Student</h3>

@if (Model is null)
{
    <p><em>Loading...</em></p>
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

    protected override async Task OnParametersSetAsync()
    {
        if (Model is not null)
        {
            return;
        }

        StudentDto? student = await SchoolService.GetStudentAsync(Id);

        if (student is not null)
        {
            Model = new StudentFormModel
            {
                Name = student.Name,
                IsActive = student.IsActive
            };
        }
    }

    private async Task Submit()
    {
        if (Model is null)
        {
            return;
        }

        StudentRequestDto requestDto = new(Model.Name, Model.IsActive);
        bool updated = await SchoolService.UpdateStudentAsync(Id, requestDto);

        if (updated)
        {
            NavigationManager.NavigateTo("/students");
        }
    }
}
```

---

## 5. Co dělá [SupplyParameterFromForm]

`[SupplyParameterFromForm]` říká Blazoru, že hodnoty modelu se mají naplnit z HTTP form postu.

V SSR/Interactive Server scénáři je to užitečné, protože:

- model je správně navázán při submitu,
- podporuje to hladké zpracování formuláře na serveru,
- funguje to přirozeně s `EditForm`.

---

## 6. Validace

Formulář je validní pouze pokud projde DataAnnotations pravidly.

Kritické části:

- `<DataAnnotationsValidator />` aktivuje validaci přes atributy modelu,
- `<ValidationSummary />` vypíše seznam chyb,
- `OnValidSubmit` volá metodu jen při validním modelu.

Díky tomu se například neodešle prázdné jméno.

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

## 7. Jak si to vyzkoušet

1. Spusťte AppHost:

```powershell
dotnet run --project .\UTB.School.AppHost
```

2. Otevřete Blazor web aplikaci.
3. Na stránce `/students` klikněte na `Create`.
4. Vytvořte nového studenta.
5. U záznamu klikněte na `Edit`, upravte data a uložte.

---

## 8. Shrnutí

V této kapitole jsme doplnili Blazor klienta o formuláře:

- vytvoření studenta (`POST /students`),
- editaci studenta (`PUT /students/{id}`),
- validační pravidla na úrovni form modelu,
- přesměrování po úspěšném uložení zpět na seznam.

---

## ❓ Kontrolní otázky

1. Jaký je rozdíl mezi `OnSubmit` a `OnValidSubmit`?
2. Proč je vhodné mít `StudentFormModel` odděleně od `StudentRequestDto`?
3. K čemu slouží `[SupplyParameterFromForm]`?
4. Jak poznáte ve službě, že `POST /students` proběhl úspěšně?
5. Proč po vytvoření nebo úpravě záznamu navigujeme zpět na `/students`?

---

## Zdroje

1. [ASP.NET Core Blazor forms overview](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/?view=aspnetcore-9.0)
2. [ASP.NET Core Blazor input components](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/input-components?view=aspnetcore-9.0)