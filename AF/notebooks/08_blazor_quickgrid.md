# 07 Blazor Web Forms

**autor: Erik Král ekral@utb.cz**

---

V této části probereme jak zobrazit velké množství dat pomocí componenty [QuickGrid](https://aspnet.github.io/quickgridsamples/).

Nejprive si musíme do projektu vložit nuget balíček [Microsoft.AspNetCore.Components.QuickGrid](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.QuickGrid).

QuickGrid může zobrazovat následující zdroje:

- In-memmory IQeryable - objekty v paměti, `IEnumerable` (List, pole, atd.) jde převést na `IQueryable` pomocí metody `AsQueryable()`. Pro menší počet objektů-
- Entity Framework IQueryable - query EF, automatické filtrování a řazení.
- Libovolná vzdálená data, například WebAPI - řazení a filtrování musí podporovat zdroj dat, například endpoint.

Pro všechny zdroje můžeme zapnout stránkování (nebo virtualizaci). 

## Zobrazení objektů v paměti

V prvním příkladu použijeme QuickGrid pro zobrazení studentů, kdy v paměti budeme mít načtené všechny studenty.

Pomocí CSS omezíme velikost gridu:

```css
.grid {
    height: 25.0rem;
    overflow-y: auto;
}

/* Sticky header while scrolling */
::deep thead {
    position: sticky;
    top: 0;
    background-color: #fff;
}
```

```razor
@page "/quickgrid1"
@using Microsoft.AspNetCore.Components.QuickGrid
@inject HttpClient Http

<h3>QuickGrid Memory Simple</h3>

@if(students is null)
{
    <p><em>Loading...</em></p>
}
else
{
    <div class="grid">
        <QuickGrid Items="Students">
            <PropertyColumn Property="@(s => s.StudentId)" Sortable="true" />
            <PropertyColumn Property="@(s => s.Jmeno)" Sortable="true">
                <ColumnOptions>
                    <div>
                        <input type="search" class="form-control-plaintext" autofocus @bind="nameFilter" @bind:event="oninput" placeholder="Jméno..." />
                    </div>
                </ColumnOptions>
            </PropertyColumn>
            <PropertyColumn Property="@(s => s.Studuje)" Sortable="true" />
            <TemplateColumn>
                <NavLink class="btn btn-primary" href="@($"students/edit/{context.StudentId}")">Edit</NavLink>
            </TemplateColumn>
        </QuickGrid>
    </div>
}

@code {
    private IQueryable<Student>? Students 
    { 
        get
        {

            return students?.Where(s => s.Jmeno.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();
        } 
    }

    private List<Student>? students;
    private string nameFilter = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        students = await Http.GetFromJsonAsync<List<Student>>("/students");
    }
}
```

## Stránkování objektů v paměti

V druhém příkladu použijeme stránkování. Stav stránkování ukládáme do proměnné `PaginationState pagination` a pro UI stránkování používáme komponentu `<Paginator State="pagination" />`. Mohli bychom si ale vytvořit vlastní UI.


```css
.grid {
    height: 28.0rem;
    overflow-y: auto;
}
```

```razor
@page "/quickgrid2"
@using Microsoft.AspNetCore.Components.QuickGrid
@inject HttpClient Http

<h3>QuickGrid Memory Pagination</h3>

@if (students is null)
{
    <p><em>Loading...</em></p>
}
else
{
    <div class="grid">
        <QuickGrid Items="Students" Pagination="pagination">
            <PropertyColumn Property="@(s => s.StudentId)" Sortable="true" />
            <PropertyColumn Property="@(s => s.Jmeno)" Sortable="true">
                <ColumnOptions>
                    <div>
                        <input type="search" class="form-control-plaintext" autofocus @bind="nameFilter" @bind:event="oninput" placeholder="Jméno..." />
                    </div>
                </ColumnOptions>
            </PropertyColumn>
            <PropertyColumn Property="@(s => s.Studuje)" Sortable="true" />
            <TemplateColumn>
                <NavLink class="btn btn-primary" href="@($"students/edit/{context.StudentId}")">Edit</NavLink>
            </TemplateColumn>
        </QuickGrid>
    </div>

    <Paginator State="pagination" />
}

@code {
    private IQueryable<Student>? Students
    {
        get
        {

            return students?.Where(s => s.Jmeno.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();
        }
    }

    private List<Student>? students;
    private string nameFilter = string.Empty;
    PaginationState pagination = new PaginationState { ItemsPerPage = 10 };

    protected override async Task OnInitializedAsync()
    {
        students = await Http.GetFromJsonAsync<List<Student>>("/students");
    }
}

```

##

Předchozí postup by byl pomalý pokud bychom zobrazovali tisíce studentů, tak by to zpomalilo prohlížeč.

## Data provider a zobrazení dat z WebApi