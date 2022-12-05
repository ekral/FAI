Návod pro Entity Framework: [Dependency inversion]([https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion))

## IoC Container

IoC Container je třída pomocí které vytváříme instance jiných tříd a zároveň při vytváření těchto instancí zjištťuje jaké jsou parametry konstruktoru a umí automaticky vkládat (inject) další instace.

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

Třída StudentListViewModel:

```csharp
public class StudentListViewModel
{
private readonly DatabaseService databaseService;

public StudentListViewModel(DatabaseService databaseService)
{
    this.databaseService = databaseService;
}

public ObservableCollection<Student> Students { get; } = new();

public async void LoadStudents()
{
    List<Student> studentsFromDatabase = await databaseService.GetAllStudents();

    foreach (Student student in studentsFromDatabase)
    {
        Students.Add(student);
    }
}
```
