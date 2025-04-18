# 06 Webový klient

**autor: Erik Král ekral@utb.cz**

---

[Studijní materiál](https://github.com/ekral/FAI/blob/master/AF/notebooks/06_blazor_klient.md)

## Webová služba (WebAPI)

1) Vložte nuget balíček `Microsoft.EntityFrameworkCore.Sqlite`.
 
2) Definujte třídu `Student`, která bude mít vlastnosti `StudentId`, `Jmeno` a `Studuje`.

3) Definujte `StudentContext` s jednou tabulkou `Studenti`. 

4) Zaregistrujte `StudentContext` do IoC kontejneru (Services).

5) Definujte metody a namapujte endpointy:
   - endpoint `/seed` vytvoří databázi a vloží do ní tři studenty.
   - endpoint `/students` vrátí všechny studenty.

6) Připojte se k endpointu `/seed` a vytvořte tím databázi.

7) Poznamenejte si URL adresu vytvořeného WebAPI.

## Webový klient

1) Vytvořte webového klienta s pomocí Blazor WebAssembly klienta a zkopírujte jeho URL adresu.

2) Vložte do webové služby konfiguraci CORS a použijte URL adresu Blazor klienta.
