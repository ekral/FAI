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


## Návratové typy pomocí TypedResults

V Minimal API v ASP.NET Core lze pomocí třídy `TypedResults` vracet silně typované HTTP odpovědi. Na rozdíl od třídy `Results`, která vrací obecný typ `IResult`, `TypedResults` vrací konkrétní typ odpovědi (např. `Ok<T>`, `Created<T>` nebo `NotFound`). Díky tomu je návratový typ endpointu přesně definovaný a může být lépe využit například při generování dokumentace API.

### Jeden status code

Pokud endpoint vrací pouze jednu odpověď, může být návratový typ přímo konkrétní typ výsledku.

```csharp
app.MapDelete("/items/{id}", async Task<NoContent> (int id, ItemService service) =>
{
    await service.DeleteAsync(id);

    return TypedResults.NoContent();
});
```

Endpoint v tomto případě vrací pouze odpověď `204 No Content` bez payloadu.

### Status code s payloadem

Pokud endpoint vrací data, používají se generické typy, například `Ok<T>` nebo `Created<T>`.

```csharp
app.MapPost("/items", async Task<Created<ItemDto>> (ItemDto item, ItemService service) =>
{
    var created = await service.CreateAsync(item);

    return TypedResults.Created($"/items/{created.Id}", created);
});
```

Tento endpoint vrací odpověď `201 Created` spolu s vytvořeným objektem v těle odpovědi.

### Více možných status kódů

Pokud endpoint může vracet více různých odpovědí, používá se typ `Results<T1, T2, ...>`, který reprezentuje sjednocení možných výsledků.

```csharp
app.MapGet("/items/{id}",
async Task<Results<Ok<ItemDto>, NotFound>> (int id, ItemService service) =>
{
    var item = await service.GetAsync(id);

    if (item == null)
        return TypedResults.NotFound();

    return TypedResults.Ok(item);
});
```

V tomto případě endpoint vrací buď `200 OK` s objektem `ItemDto`, nebo `404 NotFound`, pokud požadovaný záznam neexistuje.

---

## Příklad: Entity, DbContext, DTOs a .http soubor

Ukázky jednotlivých HTTP metod si probereme na příkladu databáze studentů. Kdy zároveň použijeme `DbContext` pro práci s databází.

### Entita

```csharp
public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required bool IsActive { get; set; }
}
```

---

### Databázový kontext

Do projektu definující DbContext musíme přidat providera pro Entity Framework Core, například nuget balíček `Microsoft.EntityFrameworkCore.Sqlite`.

```csharp
public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
}
```

### Registrace databáze v Program.cs

```csharp
builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("Data Source=students.db"));
```

---

### DTO – Data Transfer Object

DTO (Data Transfer Object) odděluje databázovou entitu od dat, která jsou poskytována klientovi přes API, nebo která klient posílá serveru. Díky tomu lze přesněji kontrolovat, jaká data API přijímá a vrací.

#### DTO pro čtení dat

`StudentDto` obsahuje data, která vrátíme klientovi:

```csharp
public record StudentDto(int Id, string Name, bool IsActive);
```

#### DTO pro zápis dat

`StudentRequestDto` obsahuje data, která přijímáme od klienta. Neobsahuje `Id`, protože to generuje databáze:

```csharp
public record StudentRequestDto(string Name, bool IsActive);
```

`StudentPatchRequest` obsahuje jen vlastnosti určené pro částečnou změnu:

```csharp
public record StudentPatchRequest(bool IsActive);
```

---

### soubor .http

HTTP metody můžeme ve Visual Studiu zavolat pomocí souboru s příponou `.http`.

Soubor s příponou `.http` bude mít na začátku nadefinovanou adresu služby, například:

```json
@Students.WebAPI_HostAddress = https://localhost:7042
```

---

## 1. POST `/dev/seed`

### Mapování

```csharp
app.MapPost("/dev/seed", Seed);
```

### Implementace

Endpoint odstraní stávající databázi (pokud existuje), vytvoří novou podle aktuálního modelu, vloží testovací data a vrátí HTTP 204 No Content.

```csharp
static async Task<NoContent> Seed(StudentContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    context.Students.AddRange(
        new Student { Name = "Jan", IsActive = true },
        new Student { Name = "Eva", IsActive = true },
        new Student { Name = "Petr", IsActive = false }
    );

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}
```

### Volání

```json
POST {{Students.WebAPI_HostAddress}}/seed

###
```

---

## 2. GET `/students`

### Mapování

```csharp
app.MapGet("/students", GetAllStudents);
```

### Implementace

Endpoint načte všechny studenty z databáze, namapuje je na `StudentDto` a vrátí HTTP 200 OK s JSON daty.

```csharp
static async Task<Ok<StudentDto[]>> GetAllStudents(StudentContext context)
{
    var students = await context.Students
        .Select(s => new StudentDto(s.Id, s.Name, s.IsActive))
        .ToArrayAsync();

    return TypedResults.Ok(students);
}
```

### Volání

```json
GET {{Students.WebAPI_HostAddress}}/students/

###
```

---

## 3. GET `/students` s filtrem pro aktivní studenty

### Mapování

```csharp
app.MapGet("/students", GetStudents);
```

### Implementace

Endpoint přijme volitelný query string parametr `isActive`. Pokud je zadán, vyfiltruje studenty podle hodnoty `IsActive`; jinak vrátí všechny. Výsledek namapuje na `StudentDto[]` a vrátí HTTP 200 OK.

```csharp
static async Task<Ok<StudentDto[]>> GetStudents(bool? isActive, StudentContext context)
{
    var query = context.Students.AsQueryable();

    if(isActive.HasValue)
    {
        query = query.Where(s => s.IsActive == isActive);           
    }

    StudentDto[] students = await query.Select(s => new StudentDto(s.Id, s.Name, s.IsActive)).ToArrayAsync();

    return TypedResults.Ok(students);
}
```

### Volání

```json
GET {{Students.WebAPI_HostAddress}}/students?isActive=true

###
```

---

## 4. GET `/students/{id}`

### Mapování

```csharp
app.MapGet("/students/{id:int}", GetStudent);
```

### Implementace

Endpoint vyhledá studenta podle primárního klíče. Pokud existuje, namapuje ho na `StudentDto` a vrátí HTTP 200 OK; pokud neexistuje, vrátí HTTP 404 Not Found.

```csharp
static async Task<Results<Ok<StudentDto>, NotFound>> GetStudent(int id, StudentContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        return TypedResults.Ok(new StudentDto(student.Id, student.Name, student.IsActive));
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

---

## 5. POST `/students`

### Mapování

```csharp
app.MapPost("/students", CreateStudent);
```

### Implementace

Endpoint přijme JSON data z těla požadavku jako `StudentRequestDto`, uloží nového studenta do databáze a vrátí HTTP 201 Created spolu s URL a `StudentDto` nového záznamu.

```csharp
static async Task<Created<StudentDto>> CreateStudent(StudentRequestDto request, StudentContext context)
{
    var student = new Student { Name = request.Name, IsActive = request.IsActive };

    context.Add(student);

    await context.SaveChangesAsync();

    return TypedResults.Created($"/students/{student.Id}", new StudentDto(student.Id, student.Name, student.IsActive));
}
```

### Volání

```json
POST {{Students.WebAPI_HostAddress}}/students
Content-Type: application/json

{
  "name": "Lenka",
  "isActive": true
}

###
```

---

## 6. PUT `/students/{id}`

### Mapování

```csharp
app.MapPut("/students/{id:int}", UpdateStudent);
```

### Implementace

Endpoint vyhledá studenta podle ID. Pokud existuje, přepíše všechny jeho vlastnosti hodnotami z `StudentRequestDto`, uloží změny a vrátí HTTP 204 No Content; pokud neexistuje, vrátí HTTP 404 Not Found.

```csharp
static async Task<Results<NoContent, NotFound>> UpdateStudent(int id, StudentRequestDto request, StudentContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        student.Name = request.Name;
        student.IsActive = request.IsActive;

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
PUT {{Students.WebAPI_HostAddress}}/students/1
Content-Type: application/json

{
  "name": "Novotna",
  "isActive": true
}

###
```

---

## 7. PATCH `/students/{id}`

### Mapování

```csharp
app.MapPatch("/students/{id}", PatchStudentActivity);
```

### Implementace

Endpoint vyhledá studenta. Pokud existuje, aktualizuje vlastnost `IsActive` podle `StudentPatchRequest`, uloží změny a vrátí HTTP 204 No Content; pokud neexistuje, vrátí HTTP 404 Not Found.

```csharp
static async Task<Results<NoContent, NotFound>> PatchStudentActivity(int id, StudentPatchRequest request, StudentContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        student.IsActive = request.IsActive;

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
Content-Type: application/json

{
    "isActive": false
}

###
```

---

## 8. DELETE `/students/{id}`

### Mapování

```csharp
app.MapDelete("/students/{id:int}", DeleteStudent);
```

### Implementace

Endpoint vyhledá studenta. Pokud existuje, odstraní ho z databáze, uloží změny a vrátí HTTP 204 No Content; pokud neexistuje, vrátí HTTP 404 Not Found.

```csharp
static async Task<Results<NoContent, NotFound>> DeleteStudent(int id, StudentContext context)
{
    if(await context.Students.FindAsync(id) is Student student)
    {
        context.Students.Remove(student);

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

---

## Úkol – Public Library API

Vytvořte Minimal Web API pro veřejnou knihovnu.

### Výchozí kód

```csharp
// Pridat nuget Microsoft.EntityFrameworkCore.Sqlite

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<LibraryContext>(opt => opt.UseSqlite("Data Source=library.db"));

var app = builder.Build();

app.MapPost("/dev/seed", Seed);

app.Run();

static async Task<NoContent> Seed(LibraryContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    var babicka = new Book { Title = "Babicka", IsArchived = false };
    var rur = new Book { Title = "R.U.R.", IsArchived = false };
    var maj = new Book { Title = "Maj", IsArchived = true };

    var loanBabicka = new Loan { Book = babicka, LoanDate = new DateOnly(2026, 3, 18) };
    var loanRur = new Loan { Book = rur, LoanDate = new DateOnly(2026, 3, 17) };

    context.Books.AddRange(babicka, rur, maj);
    context.Loans.AddRange(loanBabicka, loanRur);

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
}

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsArchived { get; set; }
    public List<Loan> Loans { get; set; } = [];
}

public class Loan
{
    public int Id { get; set; }
    public required DateOnly LoanDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
}
```

---

### Implementujte endpointy

- `POST /dev/seed` smaže a vytvoří databází a přidá do databáze tři knihy a dvě výpůjčky.
- `GET /books` vrátí všechny knihy. Vytvořte také variantu s query string parametrem IsArchived, který bude volitelně definovat zda se mají vracet jen archivované nebo nearchivované knihy. 
- `GET /books/{id}` vrátí knihu dle Id.    
- `POST /books` vytvoří novou knihu.
- `PUT /books/{id}` nahradí existující knihu jinou. 
- `DELETE /books/{id}` odstraní knihu dle Id.
- `PATCH /books/{id}` (v body pošle DTO zda `IsArchived = false` nebo `IsArchived = true`)
- `GET /loans/` vrátí všechny výpůjčky (název knihy).
- `POST /loans/` vytvoří novou výpůjčku pro knihu pokud je kniha dostupná k půjčování a není již vypůjčená.


### Další požadavky

1. Použijte SQLite databázi.  
2. Použijte DTO a definujte je s využitím recordu. 
3. Připravte `.http` soubor pro manuální testování (pouze ve Visual Studiu, jinde použijte například Postman).  

---

## ❓ Kontrolní otázky

1. Jaký je rozdíl mezi PUT a PATCH?  
2. Co vrátí metoda `GetStudent`, pokud záznam neexistuje?  
3. Jak funguje metoda `Seed` s `EnsureDeletedAsync()` a `EnsureCreatedAsync()`?  
4. Proč je vhodné používat DTO?  
5. Jak funguje dependency injection v tomto příkladu?  
6. Jaký je rozdíl mezi `FindAsync()` a `ToArrayAsync()`?

---
