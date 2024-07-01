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

Když se změní kolekce kterou zobrazujeme, například vložíme nový prvek, nebo odstraníme prvek z kolekce, tak se znovu vyrenderují i všechny elementy UI nebo komponenty. Toto chování můžeme změnit tak, že použijeme @key atribut s pomocí kterého se budou renderovat pouze elementy nebo komponenty které mají jinou hodnotu @key atributu.

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