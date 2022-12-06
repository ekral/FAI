Dokumentace: [Serialization in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)

## Serialization

IoC Container je třída pomocí které vytváříme instance jiných tříd a zároveň při vytváření těchto instancí zjistí jaké jsou parametry konstruktoru a umí automaticky vkládat (inject) další instace jako argumenty konstruktoru.

Dotnet obsahuje zabudovaný IoC kontejner, který přidáme do projektu pomocí následujího nuget balíčku:

```powershell
dotnet add package Microsoft.Extensions.DependencyInjection
```
