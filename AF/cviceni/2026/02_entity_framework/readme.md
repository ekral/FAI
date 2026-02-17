# Úkol 01: Aspire Host

*Autor: Erik Král – <ekral@utb.cz>*

V tomto cvičení si procvičíme Entity Framework.

Otevřete si solution ze složky [zadani](/zadani) a doplňte chybějící kód související s prací s databází v Entity Frameworkem.

## DbContext

1. Do projektu `UTB.Library.Db` přidejte třídu `LibraryContext` definující tabulku `Authors`.
2. Do projektu `UTB.Library.WebApi` v souboru `Program.cs` doplňte vložení třídy `LibraryContext` do IoC kontejneru.

## Reset Database

1. Do projektu `UTB.Library.DbManager` v souboru `Program.cs` doplňte:
- smazání databáze pokud existuje,
- vytvoření databáze pokdu neexistuje,
- vložení tří studentů do databáze[^1]

[^1] nezapomeňte uložit změny v contextu do databáze pomocí zavolání metody `context.SaveChangesAsync`.
