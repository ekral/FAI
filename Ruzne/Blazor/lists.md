# Zobrazování kolekcí

Kolekci dat můžeme zobrazit pomocí cyklu v jazyce C#, například ```foreach```. V následujícím příkladu zobrazíme kolekci studentů:

```razor
@page "/students"

<h3>Students</h3>

<ul>
    @foreach (Student student in Students)
    {
        <li>@student.Id : @student.Name</li>
    }
</ul>

@code {
    List<Student> Students { get; set; } = [new(1, "Alice"), new(2, "Peter"), new(3, "John")];

    record Student(int Id, string Name);
}
```
## Atribut @key

Pokud bychom vkladali nového studenta jako prvního do kolekce, tak by se změnili hodnoty všech následujícíh položek a změnil by se HTML kód v DOM pro všechny ```<li>``` tagy reprezentující jednotlivé studenty. Toto chování můžeme změnit tak, že použijeme ```@key``` atribut s pomocí kterého namapujeme element UI nebo componentu na konkrétní model. Změna se potom bude kontrolovat dle hodnoty atributu ```@key``` a v DOMu se změní pouze ```<li>``` pro jednoho studenta.

Následující příklad demonstruje použití atributu ```@key```, vyzkoušejte si kód s použitím tohoto atributu a bez použití tohoto atributu s využitím vývojářských nástrojů v prohlížeči:

```razor
@page "/students"
@rendermode InteractiveServer

<h3>Students</h3>

<ul>
    @foreach (Student student in Students)
    {
        <li @key=student.Id >@student.Id : @student.Name</li>
    }
</ul>

<button @onclick="AddStudent">Add Student</button>

@code {
    List<Student> Students { get; set; } = [new(1, "Alice"), new(2, "Peter"), new(3, "John")];

    public void AddStudent()
    {
        int id = Students.Max(s => s.Id) + 1;
        Students.Insert(0, new Student(id, "Karel"));
    }

    record Student(int Id, string Name);
}
```

## Virtualizace 

Renderování velkého množství položek na straně klienta (InteractiveServer nebo InteractiveWebassembly) může být pomalé. Proto je lepší použít virtualizaci, kdy se elementy uživatelského prostředí na řádově stránce a půl při stránkování nevytváří znovu, ale zůstavají stejné a jen se mění data které mají zobrazovat.

V prvním příkladu není virtualizace použitá, pomocí vývojářských nástrojů se podívejte, že se vyrenderuje 2000 ```<li>``` tagů.

```razor
@page "/students"
@rendermode InteractiveServer

<h3>Students</h3>

<ul>
    @foreach (Student student in Students)
    {
        <li>@student.Id : @student.Name</li>
    }
</ul>

@code {
    List<Student> Students { get; set; }

    string[] names = ["Karl", "Alice", "John", "George", "Peter"];

    public StudentsPage()
    {
        Students = Enumerable.Range(0, 2000)
                   .Select(x => new Student(x, names[Random.Shared.Next(names.Length)]))
                   .ToList();
    }

    record Student(int Id, string Name);
}
```

V následujícím příkladu je virtualizace použitá. Blazor dle výšky řádku Blazor spočítá, kolik ``<li>`` tagů se vejde na obrazovku a zbytek doplní ```<div>``` tagem s výškou odpovídající výšce zbývajících ```<li>``` tagů. Při skrolování potom mění výšku ```<div>``` tagů před seznamem a ```<div>``` tagem za seznamem. Počet ```<li>``` tagů je řádově jen desítky. Zjistěte konkrétní počet pomocí vývojářských nástrojů v prohlížeči.

```razor
@page "/students"
@rendermode InteractiveServer

<h3>Students</h3>

<ul>
    <Virtualize Items="Students">
        <li>@context.Id : @context.Name</li>
    </Virtualize>
</ul>

@code {
    List<Student> Students { get; set; }

    string[] names = ["Karl", "Alice", "John", "George", "Peter"];

    public StudentsPage()
    {
        Students = Enumerable.Range(0, 2000)
                   .Select(x => new Student(x, names[Random.Shared.Next(names.Length)]))
                   .ToList();
    }

    record Student(int Id, string Name);
}
```

---
1. [Retain element, component, and model relationships in ASP.NET Core Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/element-component-model-relationships?view=aspnetcore-8.0)
2. [ASP.NET Core Razor component virtualization](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/virtualization?view=aspnetcore-8.0)