# Cvičení: Pole grafických objektů

Vytvořte dynamické pole grafických objektů, který vykreslíte v jednom cyklu.

1) Vytvořte rodičovskou třídu `GrafickyObjekt`, která bude mít abstraktní funkci `Nakresli`.
2) Třídy `RovnostrannyTrojuhelnik` a `Ctverec` budou tuto funkci implementovat.
3) S pomocí třídy `std::vector` z knihovny `#include <vector>` vytvořte dynamické pole grafických objektů, které pak vykreslíte na plátno pomocí `range base loop` (zjednodušený cyklus `for` pro procházení objektů). Do pole vložte jak ukazatel na rovnostranný trojúhelník tak na čtverec.
