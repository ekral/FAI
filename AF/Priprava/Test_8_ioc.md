# IoC Container

Dokumentace: [Dependency injection]([https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection))

IoC Container je třída pomocí které vytváříme instance jiných tříd a zároveň při vytváření těchto instancí zjistí jaké jsou parametry konstruktoru a umí automaticky vkládat (inject) další instace jako argumenty konstruktoru.

Dotnet obsahuje zabudovaný IoC kontejner, který přidáme do projektu pomocí následujího nuget balíčku:

```powershell
dotnet add package Microsoft.Extensions.DependencyInjection
```

Nejprve si zaregistruje třídy, jejichž instance, chceme vytvářet. Zároveň zvolím jaký bude životnost vytvořených objektů. 

- Singleton znamená, že bude existovat jen jedna instance dané třídy. Kontejner tedy vždy vrátí referenci na stejný objekt.
- Transient znamé, že se vytvoří vždy nová instance třídy. Kontejner tedy vrátí vždy referenci na nový objekt.
- Scooped znamená, že se vytvoří vždy nová instance třídy pro nový request, ale v rámci jednoho requestu pak už vrací referenci na stejný objekt.


V následujícím příkladu registrujme DatabaseService jako Singleton:

```csharp
IServiceCollection serviceCollection = new ServiceCollection().AddSingleton<DatabaseService>();
```

A instance třídy DatabaseService vytváříme následujícím způsobem pomocí třídy ```ServiceProvider```. Kdy ```service1``` i ```service2``` představují referenci na stejný objekt.

```csharp
ServiceProvider provider = serviceCollection.BuildServiceProvider();

DatabaseService service1 = provider.GetRequiredService<DatabaseService>();
DatabaseService service2 = provider.GetRequiredService<DatabaseService>();
```

Příklad rozšíříme a zaregistrujeme ViewModel jako transient. 

```csharp
IServiceCollection serviceCollection = new ServiceCollection()
                .AddSingleton<DatabaseService>()
                .AddTransient<StudentListViewModel>();
```

Nyní mně kontejner vytvoří dvě instance třídy ```StudentViewModel```, ale každé instanci ```StudentViewModelu``` předá jako argument konstruktoru refenci na stejnou instanci třídy ```DatabaseService```.

```csharp
StudentListViewModel viewModel1 = provider.GetRequiredService<StudentListViewModel>();
StudentListViewModel viewModel2 = provider.GetRequiredService<StudentListViewModel>();
```

### IoC a Dependency Injection

IoC kontejner se používa společně s Dependency Injection. V následujícím příkladu máme rozhraní IDatabaseService a dvě implementace DatabaseService, která pracuje s databází a FakeDatabaseService, která slouží jen pro testování a vrací jen objekty v paměti.

V kontejneru si potom můžeme jednoduše volit konkrétní implementaci.

```csharp
IServiceCollection serviceCollection = new ServiceCollection()
  .AddSingleton<IDatabaseService, FakeDatabaseService>()
  .AddTransient<StudentListViewModel>();
```

---
Použité rozhraní a třídy.

```csharp
public interface IDatabaseService
{
    Task<List<Student>> GetAllStudents();
}
```

```csharp
public class DatabaseService : IDatabaseService
{
    public async Task<List<Student>> GetAllStudents()
    {
        await using SchoolContext schoolContext = new SchoolContext();

        List<Student> students = await schoolContext.Students.ToListAsync();

        return students;
    }
}
```

```csharp
public class FakeDatabaseService : IDatabaseService
{
    public Task<List<Student>> GetAllStudents()
    {
        List<Student> studenti = new List<Student>()
        {
            new Student() { Id = 1, Name = "Jitka"},
            new Student() { Id = 2, Name = "Oto"},
            new Student() { Id = 3, Name = "Jiri"}
        };

        return Task.FromResult(studenti);
    }
}
```
  
```csharp
public class StudentListViewModel
{
  private readonly DatabaseService databaseService;

  public StudentListViewModel(DatabaseService databaseService)
  {
      this.databaseService = databaseService;
  }
}
```
