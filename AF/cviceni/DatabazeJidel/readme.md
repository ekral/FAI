# Úkol: Databáze jídel v menze

Vytvořte databází jídel v menze.

Solution bude obsahovat projekty

1. Class Library: **Menza.Models** - bude obsahovat třídu Jidlo s Id, názvem a cenou. 
2. Class Library: **Menza.Data** - bude obsahovat context pro EntityFramework. Použijte providera pro Sqlite. Reference na projekt Menza.Models.
3. Console Application: **Menza.DataSeeder** - konzolová aplikace pro vytvoření a naplnění databáze. Reference na projekt Menza.Data.
4. ASP.NET Core Empty: **Menza.WebApi** - Webová služba, která vrátí seznam jídel. Reference na projekt Menza.Data.
5. Console Application: **Menza.ConsoleClient** - pomocí http clienta načte a zobrazí seznam jídel. Reference na projekt Menza.Models.
