# Materiál 07: QuickGrid

**autor: Erik Král ekral@utb.cz**

---

V této části probereme jak zobrazit velké množství dat pomocí componenty [QuickGrid](https://aspnet.github.io/quickgridsamples/).

Nejprive si musíme do projektu vložit nuget balíček:

```
Microsoft.AspNetCore.Components.QuickGrid
```

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

Poslední příklad používá `ItemsProvider`, kdy pro každou stránku provedeme dotaz na WebAPI. Stejně tak provádíme dotaz na WebAPI pokud se změní property `NameFilter`. 

Pro sestavení URI používáme nuget balíček:

```
    Microsoft.AspNetCore.WebUtilities
```

Nejprve si ukážeme aktualizovanou metodu WebAPI:

```csharp
public static async Task<Ok<PaginationResult>> GetStudentsPage(StudentContext context, int startIndex, int count, string? sortBy = null, string? direction = null, string? nameFilter = null)
{
    IQueryable<Student> query = context.Studenti;

    if(nameFilter is not null)
    {
        query = query.Where(s => s.Jmeno.ToLower().Contains(nameFilter.ToLower()));
    }

    if (sortBy is not null && direction is not null)
    {
        switch (direction)
        {
            case "Ascending":
                query = sortBy switch
                {
                    "StudentId" => query.OrderBy(s => s.StudentId),
                    "Jmeno" => query.OrderBy(s => s.Jmeno),
                    "Studuje" => query.OrderBy(s => s.Studuje),
                    _ => query
                };
                break;
            case "Descending":
                query = sortBy switch
                {
                    "StudentId" => query.OrderByDescending(s => s.StudentId),
                    "Jmeno" => query.OrderByDescending(s => s.Jmeno),
                    "Studuje" => query.OrderByDescending(s => s.Studuje),
                    _ => query
                };
                break;
        }
    }

    Student[] students = await query.Skip(startIndex).Take(count).ToArrayAsync();
    int total = await context.Studenti.CountAsync();

    var result = new PaginationResult(students, total);

    return TypedResults.Ok(result);
}
```

Dále máme následující css:

```css
.grid {
    height: 28.0rem;
    overflow-y: auto;
}
```

A nakonec razor kód:

```razor
@page "/quickgrid3"

@using Microsoft.AspNetCore.Components.QuickGrid
@using Microsoft.AspNetCore.WebUtilities

@inject HttpClient Http

<h3>QuickGridWebApi</h3>

<div class="grid">
    <QuickGrid ItemsProvider="gridItemsProvider" Pagination="pagination" @ref="grid">
        <PropertyColumn Property="@(s => s.StudentId)" Sortable="true" />
        <PropertyColumn Property="@(s => s.Jmeno)" Sortable="true">
            <ColumnOptions>
                <div>
                    <input type="search" class="form-control-plaintext" autofocus @bind="NameFilter" @bind:event="oninput" placeholder="Jméno..." />
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

@code {
    QuickGrid<Student>? grid;
    PaginationState pagination = new PaginationState() { ItemsPerPage = 10 };

    GridItemsProvider<Student>? gridItemsProvider;

    private string nameFilter = string.Empty;
    
    private string NameFilter
    {
        get => nameFilter;
        set
        {
            nameFilter = value;

            grid?.RefreshDataAsync();
        }
    }

    protected override void OnInitialized()
    {
        gridItemsProvider = async req =>
        {
            string uri = "/students/page";

            uri = QueryHelpers.AddQueryString(uri, "startIndex", req.StartIndex.ToString());
            uri = QueryHelpers.AddQueryString(uri, "count", (req.Count ?? 10).ToString());

            var properties = req.GetSortByProperties();

            if (properties.Count > 0)
            {
                var property = properties.First();

                bool descending = property.Direction == SortDirection.Descending ? true : false;

                uri = QueryHelpers.AddQueryString(uri, "sortBy", property.PropertyName);
                uri = QueryHelpers.AddQueryString(uri, "descending", descending.ToString());
            }

            if(!string.IsNullOrWhiteSpace(NameFilter))
            {
                uri = QueryHelpers.AddQueryString(uri, "nameFilter", NameFilter);
            }

            PaginationResult? result = await Http.GetFromJsonAsync<PaginationResult>(uri);

            if (result is null)
            {
                return GridItemsProviderResult.From<Student>([], 0);
            }

            return GridItemsProviderResult.From(result.Students, result.Total);
        };
    }

    public record PaginationResult(Student[] Students, int Total);
}
```
