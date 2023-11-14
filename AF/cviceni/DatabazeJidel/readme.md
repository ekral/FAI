# Úkol: Databáze jídel v menze

- [Tutorial: Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio).
- [How to create responses in Minimal API apps](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-8.0#typedresults-vs-results).
  
Vytvořte databází jídel v menze.

Solution bude obsahovat projekty

1. Class Library: **Menza.Models**

    - Bude obsahovat třídu Jidlo s Id, názvem a cenou.
   
3. Class Library: **Menza.Data**
  
   - Bude obsahovat třídu `MenzaContext` pro EntityFramework.
   - Použijte Entity Framework providera pro Sqlite.
   - Reference na projekt Menza.Models.
   
6. Console Application: **Menza.DataSeeder**

   - Konzolová aplikace pro vytvoření a naplnění databáze.
   - Reference na projekt Menza.Data.
   
8. ASP.NET Core Empty: **Menza.WebApi**
  
    - Webová služba, která vrátí:
        -  seznam jídel,
        -  **jedno jídlo podle Id**.
    - Reference na projekt Menza.Data.
      
11. Console Application: **Menza.ConsoleClient**

    - Pomocí http clienta načte a zobrazí:
        -  seznam jídel,
        -  **jedno jídlo v menze pomocí Id**.
    - Reference na projekt Menza.Models.
   
12. Avalonia .NET App: **Menza.DesktopClient**

    - Pomocí http clienta načte a zobrazí:
        -  **jedno jídlo v menze pomocí Id**.
    - Reference na projekt Menza.Models.
