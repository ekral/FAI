Návod pro Entity Framework: [Dependency inversion]([https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion))

## IoC Container

IoC Container je třída pomocí které vytváříme instance jiných tříd a zároveň při vytváření těchto instancí zjištťuje jaké jsou parametry konstruktoru a umí automaticky vkládat (inject) další instace.

Dotnet obsahuje zabudovaný IoC kontejner, který přidáme do projektu pomocí následujího nuget balíčku:

```powershell
dotnet add package Microsoft.Extensions.DependencyInjection
```
