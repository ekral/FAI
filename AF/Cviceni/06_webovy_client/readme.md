# 06 Webový klient

**autor: Erik Král ekral@utb.cz**

---

## Webová služba (WebAPI)

1) Definujte třídu `Student`, která bude mít vlastnosti `StudentId`, `Jmeno` a `Studuje`.

2) Vložte nuget balíček `Microsoft.EntityFrameworkCore.Sqlite Definujte` `StudentContext` s jednou tabulkou `Studenti`. 

3) Definujte metody a namapujte endpointy:
   - endpoint `/seed` vytvoří databázi a vloží do ní tři studenty.
   - endpoint `/students` vrátí všechny studenty.

4) Připojte se k endpointu `/seed` a vytvořte tím databázi.

5) Poznamenejte si URL adresu vytvořeného WebAPI.

## Webový klient

Vytvořte webového klienta s pomocí Standalone WebAssembly Blazor klienta.