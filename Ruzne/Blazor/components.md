# Componenty

V Blazoru rozlišujeme routable a Non-routable komponenty. Routable komponenta má direktivu @page, zatímco non-routable tuto direktivu nemá. Non-routable komponenta se potom používá v jiných komponentách.

Následuje příklad non-routable komponenty ```Student.razor```:

```razor
<h3>Non-routable Component</h3>

@code {
 
}
```

Následující příklad představuje routable komponentu ```Home.razor``` a použití komponenty ```Student```:

```razor
@page "/"

<PageTitle>Home</PageTitle>

<Student/>
```

Komponenta může mít parametry, například následující student má jako parametry ```Id```a ```Name```

```razor
<h3>Student</h3>

@Id: @Name

@code {
    [Parameter]
    public int Id { get; set; }

    [Parameter]
    public string? Name { get; set; }
}
```

Použití komponenty s parametry je potom následující:

```razor
@page "/"

<PageTitle>Home</PageTitle>

<Student Id="1" Name="Karl"/>
```
## Route Parameter

Parametr může být zadaný i v cestě, následující příklad ukazuje routable komponentu Subject s route parametrem ```Id``` zadaným v cestě, kdy je specifikovaný i Route Constraint ```int```. Route parametr musí mít stejný název jako property označená atributem ```[Parametr]``` bez ohledu na velká a malá písmena. Query parametry mohou být i volitelné, potom by se za názvem route parametru uvedl otazník, například ```@page "/subject/{id:int?}"```. Adresa následující komponenty včetně route parametru je potom například ```https://localhost:7299/subject/3```.

```razor
@page "/subject/{id:int}"
<h3>Subject</h3>

@Id

@code {
    [Parameter]
    public int Id { get; set; }
}
```

## Query String Parameters

Parametr může být zadaný i v dotazu, například ```https://localhost:7299/subject/1?Filter=English```. V následujícím kódu potom definujeme property Filter s atributem ```[SupplyParameterFromQuery]```.

```razor
@page "/subject/{id:int}"
<h3>Subject</h3>

@Id: @Filter

@code {
    [Parameter]
    public int Id { get; set; }

    [SupplyParameterFromQuery]
    public string? Filter { get; set; }
}
```

## Cascading Parameters


---
1. [ASP.NET Core Razor components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-8.0)
2. [Route Parameters](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-8.0#route-parameters)
3. [Query Strings](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-8.0#query-strings)
4. [ASP.NET Core Blazor cascading values and parameters](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-8.0)