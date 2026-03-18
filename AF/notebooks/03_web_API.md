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

### Databázový kontext

Do projektu definující DbContext musíme přidat providera pro Entity Framework Core, například nuget balíček `Microsoft.EntityFrameworkCore.Sqlite`.

```csharp
public class StudentContext(DbContextOptions<StudentContext> options)
    : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
}
```

### Registrace databáze v Program.cs

```csharp
builder.Services.AddDbContext<StudentContext>(opt => opt.UseSqlite("Data Source=students.db"));
```

---

## DTO – Data Transfer Object

DTO (Data Transfer Object) odděluje databázovou entitu od dat, která jsou poskytována klientovi přes API, nebo která klient posílá serveru. Díky tomu lze přesněji kontrolovat, jaká data API přijímá a vrací.

### DTO pro čtení dat

`StudentDto` obsahuje data, která vrátíme klientovi:

```csharp
public record StudentDto(int Id, string Name, bool IsActive);
```

### DTO pro zápis dat

`StudentRequest` obsahuje data, která přijímáme od klienta. Neobsahuje `Id`, protože to generuje databáze:

```csharp
public record StudentRequest(string Name, bool IsActive);
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

### Více možných status codů

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

V tomto případě endpoint může vrátit buď `200 OK` s objektem `ItemDto`, nebo `404 NotFound`, pokud požadovaný záznam neexistuje.

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
- uloží změny,
- vrátí HTTP 201 (Created) pomocí `TypedResults`.

---

## 2. GET `/students`

### Mapování

```csharp
app.MapGet("/students", WebApiVersion1.GetAllStudents);
```

### Implementace

```csharp
public static async Task<Ok<StudentDto[]>> GetAllStudents(StudentContext context)
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
public static async Task<Ok<StudentDto[]>> GetActiveStudents(StudentContext context)
{
    var students = await context.Students
        .Where(s => s.IsActive)
        .Select(s => new StudentDto(s.Id, s.Name, s.IsActive))
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
public static async Task<Results<Ok<StudentDto>, NotFound>> GetStudent(int id, StudentContext context)
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
public static async Task<Created<StudentDto>> CreateStudent(StudentRequest request, StudentContext context)
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
public static async Task<Results<NoContent, NotFound>> UpdateStudent(int id, StudentRequest request, StudentContext context)
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

### Co kód dělá

- vyhledá studenta podle ID,  
- pokud existuje, přepíše všechny jeho vlastnosti,  
- uloží změny,  
- vrátí HTTP 204 No Content,
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
- vrátí HTTP 204 No Content,
- pokud neexistuje, vrátí HTTP 404 Not Found.

---

## 8. PATCH `/students/{id}`

### Mapování

```csharp
app.MapPatch("/students/{id}", WebApiVersion1.DeactivateStudent);
```

### Implementace

```csharp
public static async Task<Results<NoContent, NotFound>> DeactivateStudent(int id, StudentContext context)
{
    if (await context.Students.FindAsync(id) is Student student)
    {
        student.IsActive = false;

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
- vrátí HTTP 204 No Content,
- pokud neexistuje, vrátí HTTP 404 Not Found.

---

## Závěrečný úkol – Public Library API

Vytvořte Minimal Web API pro veřejnou knihovnu.

### Výchozí kód

```csharp
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
    public required DateOnly LoanDate{ get; set; }
    public DateOnly? ReturnDate{ get; set; }
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
