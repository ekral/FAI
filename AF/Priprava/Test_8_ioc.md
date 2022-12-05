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

  
 ```csharp
IServiceCollection serviceCollection = new ServiceCollection().AddSingleton<DatabaseService>();
```
