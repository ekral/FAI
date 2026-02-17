# Úkol 01: Aspire Host

*Autor: Erik Král – <ekral@utb.cz>*

V tomto cvičení si procvičíme Entity Framework.

Otevřete si solution ze složky [zadani](/zadani) a doplňte chybějící kód související s prací s databází v Entity Frameworkem.

## DbContext

1. Do projektu `UTB.Library.Db` přidejte třídu `LibraryContext` definující tabulku `Authors`.
2. Do projektu `UTB.Library.WebApi` v souboru `Program.cs` doplňte vložení třídy `LibraryContext` do IoC kontejneru.
2. Do projektu `UTB.Library.DbManager` v souboru `Program.cs` doplňte vložení třídy `LibraryContext` do IoC kontejneru.

## Reset Database

1. Do projektu `UTB.Library.DbManager` v souboru `Program.cs` doplňte:
- smazání databáze pokud existuje,
- vytvoření databáze pokdu neexistuje,
- vložení tří studentů do databáze

> nezapomeňte uložit změny v contextu do databáze pomocí zavolání metody `context.SaveChangesAsync`.

## CRUD

Do projektu `UTB.Library.WebApi` doplňte těla metod pro:

1. Přidání nového autora do databáze.
2. Vrácení všech autorů z databáze.
3. Vrácení jednoho autora podle id (už je implementováno, jen ho zkontrolujte).
4. Změna autora v databázi.
5. Odstranění autora z databáze.