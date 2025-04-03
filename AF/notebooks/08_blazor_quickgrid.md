# 07 Blazor Web Forms

**autor: Erik Král ekral@utb.cz**

---

V této části probereme jak zobrazit velké množství dat pomocí componenty [QuickGrid](https://aspnet.github.io/quickgridsamples/).

V následujícím příkladu použijeme QuickGrid pro zobrazení studentů, kdy v paměti budeme mít načtené všechny studenty.

Nejprive si musíme do projektu vložit nuget balíček `Microsoft.AspNetCore.Components.QuickGrid`.

Použití potom bude následující, kdy zobrazíme všechny studenty v databázi, studentů můžou být stovky až tisíce.

```razor
@page "/quickgridmemory"
@using Microsoft.AspNetCore.Components.QuickGrid
@inject HttpClient Http

<h3>StudentsQuickGridMemory</h3>

<QuickGrid Items="studenti">
    <PropertyColumn Property="@(s => s.StudentId)" Title="Id" />
    <PropertyColumn Property="@(s => s.Jmeno)" Title="Jméno" />
    <PropertyColumn Property="@(s => s.Studuje)" Title="Studuje" />
</QuickGrid>

@code {
    private IQueryable<Student>? studenti;

    protected override async Task OnInitializedAsync()
    {
        studenti = (await Http.GetFromJsonAsync<Student[]>("students"))?.AsQueryable();
    }

}
```