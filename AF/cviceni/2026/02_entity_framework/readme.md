# Úkol 01: Aspire Host

*Autor: Erik Král – <ekral@utb.cz>*

V tomto cvičení si procvičíme Entity Framework a jeho použití ve WebAPI a Aspire.

## 📋 Postup

Otevřete si solution ze složky [zadani](./zadani) a doplňte chybějící kód související s prací s databází v Entity Frameworku.

### DbContext

1. Do projektu `UTB.Library.Db` přidejte třídu `LibraryContext` definující tabulku `Authors`.
2. Do projektu `UTB.Library.WebApi` v souboru `Program.cs` doplňte vložení třídy `LibraryContext` do IoC kontejneru.
2. Do projektu `UTB.Library.DbManager` v souboru `Program.cs` doplňte vložení třídy `LibraryContext` do IoC kontejneru.

### Reset Database

1. Do projektu `UTB.Library.DbManager` v souboru `Program.cs` doplňte:
- smazání databáze pokud existuje,
- vytvoření databáze pokdu neexistuje,
- vložení tří studentů do databáze

> nezapomeňte uložit změny v contextu do databáze pomocí zavolání metody `context.SaveChangesAsync`.

### CRUD

Do projektu `UTB.Library.WebApi` doplňte těla metod pro:

1. Přidání nového autora do databáze.
2. Vrácení všech autorů z databáze.
3. Vrácení jednoho autora podle id (už je implementováno, jen ho zkontrolujte).
4. Změna autora v databázi.
5. Odstranění autora z databáze.

## ✅ Výsledek

Po dokončení úkolu:

- HTTP Command reset-db resetuje a seeduje databázi,
- WebAPI umožňuje:
    - přidat nového autora do databáze,
    - vrátit všechny autory z databáze,
    - vrátit jednoho autora podle id,
    - změnit autora v databázi,
    - odstranit autora z databáze.
- Všechny testy v projektu úspěšně projdou.

> Poznámka: endpointy můžeme otestovat pomocí souboru `UTB.Library.WebAPI.http` v projektu `UTB.Library.WebApi` nebo například pomocí aplikace [PostMan](https://www.postman.com/) pokud nepoužíváte Visual Studio .
