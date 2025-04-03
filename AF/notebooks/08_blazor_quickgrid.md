# 07 Blazor Web Forms

**autor: Erik Král ekral@utb.cz**

---

V této části probereme jak zobrazit velké množství dat pomocí componenty [QuickGrid](https://aspnet.github.io/quickgridsamples/).

V následujícím příkladu použijeme QuickGrid pro zobrazení studentů, kdy v paměti budeme mít načtené všechny studenty.

Nejprive si musíme do projektu vložit nuget balíček `Microsoft.AspNetCore.Components.QuickGrid`.

QuickGrid může zobrazovat následující zdroje:

- In-memmory IQeryable - objekty v paměti, `IEnumerable` (List, pole, atd.) jde převést na `IQueryable` pomocí metody `AsQueryable()`. Pro menší počet objektů-
- Entity Framework IQueryable - query EF, automatické filtrování a řazení.
- Libovolná vzdálená data, například WebAPI - řazení a filtrování musí podporovat zdroj dat, například endpoint.

Použití potom bude následující, kdy zobrazíme všechny studenty v databázi, studentů můžou být stovky až tisíce.

Pomocí CSS omezíme velikost gridu:

```css
.grid {
    height: 25rem;
    overflow-y: auto;
}
```

```razor
@page "/quickgridmemory"
@using Microsoft.AspNetCore.Components.QuickGrid
@inject HttpClient Http

<h3>StudentsQuickGridMemory</h3>

<div class="grid">
    <QuickGrid Items="studenti">
        <PropertyColumn Property="@(s => s.StudentId)" Title="Id" />
        <PropertyColumn Property="@(s => s.Jmeno)" Title="Jméno" />
        <PropertyColumn Property="@(s => s.Studuje)" Title="Studuje" />
    </QuickGrid>
</div>

@code {
    private IQueryable<Student>? studenti;

    protected override async Task OnInitializedAsync()
    {
        studenti = (await Http.GetFromJsonAsync<Student[]>("students"))?.AsQueryable();
    }

}
```