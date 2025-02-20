# 03 Minimal Web API

**autor: Erik Král ekral@utb.cz**

---

V tomto materiálu si probereme práci s webovými službami s pomocí Minimal Web API.

Web API (Web Application Programming Interface) je sada pravidel a protokolů umožňující komunikovat programům prostřednictvím internetu. REST (Representation State Transfer) je druh Web API a představuj architektonický styl použivající standartní HTTP metody (GET, POST, PUT a DELETE) zpřístpňující endpoity identifikvané pomocí URI. Pro přenos dat využívá přitom především format JSON.

Minimal Web API je zjednodušený způsob tvorby HTTP API pomocí ASP.NET Core.

https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0#typedresults-vs-results

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-9.0


Následující kód vrátí na metodu GET text "Hello World". Představuje nejjednodušší program v Minimal Web API.

```csharp
var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.MapGet("/", () => $"Hello World.");

app.Run();
```




```csharp
public class Student
{
    public int StudentId {get; set;}
    public required string Jmeno {get; set;}
    public required bool Studuje {get;set;}
}
```
