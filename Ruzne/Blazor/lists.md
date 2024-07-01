# Zobrazovnání kolekcí

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

Pokud bychom vkladali studenta jako prvního do kolekce, tak by se změnili hodnoty všech následujícíh položek a změnil by se HTML kód v DOM pro všechny html elementy. Místo kódu představující jednoho studenta by se tedy znovu vykreslili všichni studenti. Toto chování můžeme změnit tak, že použijeme @key atribut s pomocí kterého namapujeme element UI nebo componentu na konkrétní model, změna se potom bude kontrolovat dle Id a v DOMu se změní pouze HTML kód pro jednoho studenta.

Následující příklad demonstruje použití atributu @key:

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

Pokud je ale počet prvků větší, tak je lepší použít virtualizaci, díky které se elementy uživatelského prostředí při stránkování nevytváří znova, ale zůstavají stejné a jen se mění jejich property.

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
    List<Student> Students { get; set; } = [new(1, "Alice"), new(2, "Peter"), new(3, "John")];

    record Student(int Id, string Name);
}
```