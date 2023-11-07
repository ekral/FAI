# Úkol: Databáze jídel v menze

Vytvořte databází jídel v menze.

Solution bude obsahovat projekty

- Menza.Models - bude obsahovat třídu Jidlo s Id, názvem a cenou. 
- Menza.Data - bude obsahovat context pro EntityFramework. Použijte providera pro Sqlite.
- Menza.DataSeeder - konzolová aplikace pro vytvoření a naplnění databáze.
- Menza.WebApi - Webová služba, která vrátí seznam jídel.
- Menza.ConsoleClient - pomocí http clienta načte a zobrazí seznam jídel.
