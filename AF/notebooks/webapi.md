# Minimal Web API – studijní materiál (bakalářské studium)

**autor: Erik Král ekral@utb.cz**

## 🎯 Definice

- Web API (Web Application Programming Interface) je sada pravidel a protokolů umožňující komunikovat programům prostřednictvím internetu. 
- REST (Representation State Transfer) je druh Web API a představuj architektonický styl použivající standartní HTTP metody (GET, POST, PUT, PATCH a DELETE) zpřístupňující endpoity identifikvané pomocí URI. Pro přenos dat využívá přitom především format JSON.
- [Minimal Web API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api) je moderní framework, který umožňuje vytvářet REST služby bez controllerů.

Používané HTTP metody:

- `GET` – čtení dat  
- `POST` – vytvoření dat  
- `PUT` – úplná aktualizace  
- `PATCH` – částečná aktualizace  
- `DELETE` – odstranění dat  

---

Následující kód vrátí na metodu GET text "Hello World". Představuje nejjednodušší program v Minimal Web API.

```csharp
var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.MapGet("/", () => "Hello World.");

app.Run();
```

---

## Datový model a DbContext

### Entita

```csharp
public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required bool IsActive { get; set; }
}
```

### Databázový kontext

Do projektu definující DbContext musíme přidat providera pro Entity Framework Core, například nuget balíček `Microsoft.EntityFrameworkCore.Sqlite`.

```csharp
public class StudentContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
}
```

### Registrace databáze v Program.cs

```csharp
builder.Services.AddDbContext(opt => opt.UseSqlite("Data Source=students.db"));
```

---

## Třída s implementací endpointů a soubor .http

Uvedené metody budou definovány v následující třídě.

```csharp
public static class WebApiVersion1
{
}
```

HTTP metody můžeme ve Visual Studiu zavolat pomocí souboru s příponou `.http`.

Soubor s příponou `.http` bude mít na začátku nadefinovanou adresu služby, například:

```json
@Students.WebAPI_HostAddress = https://localhost:7042
```

---

## 1. POST `/seed`

### Mapování

```csharp
app.MapPost("/seed", WebApiVersion1.Seed);
```

---

### Implementace

```csharp
public static async Task<Created> Seed(StudentContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    context.Students.AddRange(
        new Student { Name = "Jan", IsActive = true },
        new Student { Name = "Eva", IsActive = true },
        new Student { Name = "Petr", IsActive = false }
    );

    await context.SaveChangesAsync();

    return TypedResults.Created();
}
```
### Volání

```json
POST {{Students.WebAPI_HostAddress}}/seed

###
```

### Co kód dělá

- odstraní databázi (pokud existuje),  
- vytvoří novou databázi podle aktuálního modelu,  
- vloží testovací data,  
- uloží změny.

---

## 2. GET `/students`

### Mapování

```csharp
app.MapGet("/students", WebApiVersion1.GetAllStudents);
```

### Implementace

```csharp
public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
{
    return TypedResults.Ok(await context.Studenti.ToArrayAsync());
}
```

### Volání

```json
GET {{Students.WebAPI_HostAddress}}/students/

###
```

### Co kód dělá

- načte všechny studenty z databáze,  
- převede je do pole,  
- vrátí HTTP 200 OK s JSON daty.

---

## 3. GET `/students/active`

### Mapování

```csharp
app.MapGet("/students/active", WebApiVersion1.GetActiveStudents);
```

### Implementace

```csharp
public static async Task<Ok<Student[]>> GetActiveStudents(StudentContext context)
{
    var students = await context.Students
                                .Where(s => s.IsActive)
                                .ToArrayAsync();

    return TypedResults.Ok(students);
}
```

### Volání

```json
GET {{Students.WebAPI_HostAddress}}/Students/Active

###
```

### Co kód dělá

- vyfiltruje pouze aktivní studenty,  
- převede je do pole,  
- vrátí HTTP 200 OK.

---

## 4. GET `/students/{id}`

### Mapování

```csharp
app.MapGet("/students/{id}", WebApiVersion1.GetStudent);
```

### Implementace

```csharp
public static async Task<Results<Ok<Student>, NotFound>> GetStudent(int id, StudentContext context)
{
    if (await context.Studenti.FindAsync(id) is Student student)
    {
        return TypedResults.Ok(student);
    }
    else
    {
        return TypedResults.NotFound();
    }
}
```

### Volání

```json
GET {{Students.WebAPI_HostAddress}}/students/1

###
```

### Co kód dělá

- vyhledá studenta podle primárního klíče,  
- pokud existuje, vrátí HTTP 200 OK,  
- pokud neexistuje, vrátí HTTP 404 Not Found.

---

## 5. POST `/students`

### Mapování

```csharp
app.MapPost("/students", WebApiVersion1.CreateStudent);
```

### Implementace

```csharp
public static async Task<Created<Student>> CreateStudent(Student student, StudentContext context)
{
    context.Add(student);

    await context.SaveChangesAsync();

    return TypedResults.Created($"/Students/GetStudent/{student.StudentId}", student);
}
```

### Volání

```json
POST {{Students.WebAPI_HostAddress}}/Students
Content-Type: application/json

{
  "id": 0,
  "name": "Lenka",
  "isactive": true
}

###
```

### Co kód dělá

- přijme JSON data z těla požadavku,  
- uloží nového studenta do databáze,  
- vrátí HTTP 201 Created s adresou nového záznamu.

---

## 6. PUT `/students/{id}`

### Mapování

```csharp
app.MapPut("/students/{id}", WebApiVersion1.UpdateStudent);
```

### Implementace

```csharp
public static async Task<Results<NoContent, NotFound>> UpdateStudent(int id, Student inputStudent, StudentContext context)
{
    if (await context.Studenti.FindAsync(id) is Student student)
    {
        student.Jmeno = inputStudent.Jmeno;
        student.Studuje = inputStudent.Studuje;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
```

### Volání

```json
PUT {{Students.WebAPI_HostAddress}}/students/1
Content-Type: application/json

{
  "studentId": 1,
  "jmeno": "Novotna",
  "studuje": true
}

###
```

### Co kód dělá

- vyhledá studenta podle ID,  
- pokud existuje, přepíše všechny jeho vlastnosti,  
- uloží změny,  
- vrátí HTTP 204 No Content.
- pokud neexistuje, vrátí HTTP 404 Not Found.

---

## 7. DELETE `/students/{id}`

### Mapování

```csharp
app.MapDelete("/students/{id}", WebApiVersion1.DeleteStudent);
```

### Implementace

```csharp
public static async Task<Results<NoContent, NotFound>> DeleteStudent(int id, StudentContext context)
{
    if(await context.Studenti.FindAsync(id) is Student student)
    {
        context.Remove(student);

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}
```

### Volání

```json
DELETE {{Students.WebAPI_HostAddress}}/students/1

###
```

### Co kód dělá

- vyhledá studenta,  
- pokud existuje, odstraní ho z databáze,  
- uloží změny,  
- vrátí HTTP 204 No Content.
- pokud neexistuje, vrátí HTTP 404 Not Found.

---

## 8. PATCH `/students/{id}`

### Mapování

```csharp
app.MapPatch("/students/{id}", WebApiVersion1.DeactivateStudent);
```

### Implementace

```csharp
public static async Task<Results<NoContent, NotFound> DeactivateStudent(int id, StudentContext context)
{
    if (await context.Studenti.FindAsync(id) is Student student)
    {
        student.Studuje = false;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}
```

### Volání

```json
PATCH {{Students.WebAPI_HostAddress}}/students/1

###
```

### Co kód dělá

- vyhledá studenta,  
- pokud existuje, nastaví `IsActive = false`,  
- uloží změny,  
- vrátí HTTP 204 No Content.
- pokud neexistuje, vrátí HTTP 404 Not Found.

---

## DTO – Data Transfer Object

DTO odděluje databázovou entitu od dat poskytovaných přes API.

### DTO s primárním konstruktorem

```csharp
public record StudentDto(int StudentId, string Name);
```

### Použití v endpointu

```csharp
app.MapGet("/students-dto", async (StudentContext context) =>
{
    return TypedResults.Ok(await context.Students
        .Select(s => new StudentDto(s.StudentId, s.Name))
        .ToArrayAsync());
});
```

### Co kód dělá

- vrací pouze vybrané vlastnosti,  
- odděluje databázový model od API kontraktu.

---

## ❓ Kontrolní otázky

1. Jaký je rozdíl mezi PUT a PATCH?  
2. Co vrátí metoda `GetStudent`, pokud záznam neexistuje?  
3. Jak funguje metoda `Seed` s `EnsureDeletedAsync()` a `EnsureCreatedAsync()`?  
4. Proč je vhodné používat DTO?  
5. Jak funguje dependency injection v tomto příkladu?  
6. Jaký je rozdíl mezi `FindAsync()` a `ToArrayAsync()`?

---

## Závěrečný úkol – Public Library API

Vytvořte Minimal Web API pro veřejnou knihovnu.

### Entita

```csharp
public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public bool IsAvailable { get; set; }
}
```

### Implementujte endpointy

- `POST /seed`  
- `GET /books`  
- `GET /books/available`  
- `GET /books/{id}`  
- `POST /books`  
- `PUT /books/{id}`  
- `DELETE /books/{id}`  
- `PATCH /books/{id}` (nastaví `IsAvailable = false`)

### Další požadavky

1. Použijte SQLite databázi.  
2. Vytvořte `BookDto` pomocí primárního konstruktoru.  
3. Připravte `.http` soubor pro testování.  
4. Ošetřete situaci, kdy záznam neexistuje.  
5. Implementaci umístěte do samostatné statické třídy.