# 06 Webový klient

**autor: Erik Král ekral@utb.cz**

---

[Studijní materiál](https://github.com/ekral/FAI/blob/master/AF/notebooks/06_blazor_klient.md)

## Webová služba (WebAPI)

1) Definujte třídu `Student`, která bude mít vlastnosti `StudentId`, `Jmeno` a `Studuje`.

2) Vložte nuget balíček `Microsoft.EntityFrameworkCore.Sqlite Definujte` `StudentContext` s jednou tabulkou `Studenti`. 

3) Zaregistrujte `StudentContext` do IoC kontejneru (Services).

4) Definujte metody a namapujte endpointy:
   - endpoint `/seed` vytvoří databázi a vloží do ní tři studenty.
   - endpoint `/students` vrátí všechny studenty.

5) Připojte se k endpointu `/seed` a vytvořte tím databázi.

6) Poznamenejte si URL adresu vytvořeného WebAPI.

## Webový klient

1) Vytvořte webového klienta s pomocí Blazor WebAssembly klienta a zkopírujte jeho URL adresu.

2) Vložte do webové služby konfiguraci CORS a použijte URL adresu Blazor klienta.
