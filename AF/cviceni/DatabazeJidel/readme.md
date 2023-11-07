# Úkol: Databáze jídel v menze

Vytvořte databází jídel v menze.

Solution bude obsahovat projekty

1. Class Library: **Menza.Models**

    - Bude obsahovat třídu Jidlo s Id, názvem a cenou.
   
3. Class Library: **Menza.Data**
  
   - Bude obsahovat context pro EntityFramework.
   - Použijte providera pro Sqlite.
   - Reference na projekt Menza.Models.
   
6. Console Application: **Menza.DataSeeder**

   - Konzolová aplikace pro vytvoření a naplnění databáze.
   - Reference na projekt Menza.Data.
   
8. ASP.NET Core Empty: **Menza.WebApi**
  
    - Webová služba, která vrátí seznam jídel.
    - Reference na projekt Menza.Data.
      
11. Console Application: **Menza.ConsoleClient**

    - Pomocí http clienta načte a zobrazí seznam jídel.
    - Reference na projekt Menza.Models.
