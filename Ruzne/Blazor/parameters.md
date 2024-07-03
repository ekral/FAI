# Component Parameters

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

Další typ parametru můžeme použív pokud máme hierarchii komponent a chceme mít parametr, který bude přístupný v celé hierarchii. V následujícím příkladu máme komponenty ```Home```, ```Component1```, ```Component2```, které jsou do sebe vnořené.

```CascadingValue``` se propojuje pomocí typu, v našem případě jde o typ ```School```:

```razor
    public class School
    {
        public int NumberOfStudents { get; set; }
    }
```
V ```Home``` page definujeme ```CasadingValue``` ```School```:

```razor
@page "/"
@rendermode InteractiveServer

<PageTitle>Home</PageTitle>

<button @onclick="Zvys">Zvys</button>

<CascadingValue Value="@School">
    <Component1/>
</CascadingValue>

@code {
    public School? School { get; set; } = new() { NumberOfStudents = 5 };

    public void Zvys()
    {
        if (School != null)
        {
            ++School.NumberOfStudents;
        }
    }
}
```
```Component1``` používá ```Component2```:

```razor
<h3>Component1</h3>

<Component2/>

@code {
 
}
```

V ```Component2``` použijeme ```CascadingValue``` s pomocí atributu ```[CascadingParameter]```.

```razor
<h3>Component2</h3>

@SchoolParameter?.NumberOfStudents

@code {
    [CascadingParameter]
    public School? SchoolParameter {get; set; }
}
```

```CascadeParameter``` může být taky pojmenovaný, například ```<CascadingValue Value="@School" Name="CascadeParamSchool">```, tak jak je ukázané v následujícím kódu:

```razor
@page "/"
@rendermode InteractiveServer

<PageTitle>Home</PageTitle>

<button @onclick="Zvys">Zvys</button>

<CascadingValue Value="@School" Name="CascadeParamSchool">
    <Component1/>
</CascadingValue>

@code {
    public School? School { get; set; } = new() { NumberOfStudents = 5 };

    public void Zvys()
    {
        if (School != null)
        {
            ++School.NumberOfStudents;
        }
    }
}
```

Kdy jméno potom použijeme v atributu ```[CascadingParameter(Name = "CascadeParamSchool")]```:

```razor
<h3>Component2</h3>

@SchoolParameter?.NumberOfStudents

@code {
    [CascadingParameter(Name = "CascadeParamSchool")]
    public School? SchoolParameter {get; set; }
}
```


---
1. [ASP.NET Core Razor components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-8.0)
2. [Route Parameters](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-8.0#route-parameters)
3. [Query Strings](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-8.0#query-strings)
4. [ASP.NET Core Blazor cascading values and parameters](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-8.0)