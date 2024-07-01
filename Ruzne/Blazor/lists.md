# Zobrazování kolekcí

Kolekci dat můžeme zobrazit pomocí cyklu v jazyce C#, například ```foreach``. V následujícím příkladu zobrazíme kolekci studentů:

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

Pokud bychom vkladali studenta jako prvního do kolekce, tak by se změnili hodnoty všech následujícíh položek a změnil by se HTML kód v DOM pro všechny html elementy. Místo kódu představující jednoho studenta by se tedy znovu vykreslili všichni studenti. Toto chování můžeme změnit tak, že použijeme @key atribut s pomocí kterého namapujeme element UI nebo componentu na konkrétní model, změna se potom bude kontrolovat dle Id a v DOMu se změní pouze HTML kód pro jednoho studenta.

Následující příklad demonstruje použití atributu ```@key```, vyzkoušejte si kód s použitím tohoto atributu a bez použití tohoto atributu s využitím vývojářských nástrojů v prohlížeči:

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

<button @onclick="AddStudent">Add Student</button>

@code {
        List<Student> Students { get; set; } = [new(1, "Alice"), new(2, "Peter"), new(3, "John")];

        public void AddStudent()
        {
            int id = Students.Max(s => s.Id) + 1;
            Students.Insert(2, new Student(id, "Karel"));
        }

        record Student(int Id, string Name);
}
```
## Virtualizace 

Renderování velkého množství položek může být pomale. Proto je, lepší použít virtualizaci, kdy se elementy uživatelského prostředí při stránkování nevytváří znova, ale zůstavají stejné a jen se mění data které mají zobrazovat.

V prvním příkladu není virtualizace použitá, pomocí vývojářských nástrojů se podívejte, že se vyrenderuje 2000 <li> tagů.

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
    List<Student> Students { get; set; } = new();

    public StudentsPage()
    {
        for (int i = 0; i < 2000; i++)
        {
            string name = Random.Shared.Next(4) switch
            {
                0 => "Karl",
                1 => "Alice",
                2 => "John",
                _ => "George"
            };

            Students.Add(new(i, name));
        }
    }
    record Student(int Id, string Name);
}
```

V následujícím příkladu je virtualizace použití, dle výšky řádku Blazor spočítá, kolik <li> tagů se vejde na obrazovku a zbytek doplní <div> tagem s výškou odpovídající výšce zbývajících <li> tagů. Při skrolování potom mění výšku <div> tagů před seznamem a <div> tagem za seznamem. Počet <li> tagů zůstává stejný, řádově jen desítky. Zjistěte konkrétní počet pomocí vývojářských nástrojů v prohlížeči.

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
    List<Student> Students { get; set; } = new();

    public StudentsPage()
    {
        for (int i = 0; i < 2000; i++)
        {
            string name = Random.Shared.Next(4) switch
            {
                0 => "Karl",
                1 => "Alice",
                2 => "John",
                _ => "George"
            };

            Students.Add(new(i, name));
        }
    }
    record Student(int Id, string Name);
}
```