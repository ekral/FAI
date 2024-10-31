# Úkol: Práce s operátory `new` a `delete` v C++

## Cíl:
Cílem úkolu je pochopit a implementovat použití operátorů `new` a `delete` v jazyce C++. Důraz bude kladen na dynamickou alokaci paměti a správu této paměti.

## Zadání:
1. **Vytvořte třídu**:
   - Název třídy: `Student`
   - Atributy: 
     - `std::string name` (jméno studenta)
     - `int age` (věk studenta)
   - Metody:
     - Konstruktor s parametry pro inicializaci jména a věku.
     - Destruktor, který vypíše zprávu o zničení objektu.

2. **Dynamická alokace**:
   - Vytvořte program, který:
     - Požádá uživatele o vstup jména a věku studenta.
     - Pomocí operátoru `new` vytvoří instanci třídy `Student` a inicializuje ji zadanými hodnotami.
     - Vypíše informace o studentovi (jméno a věk).
   
3. **Uvolnění paměti**:
   - Po použití objektu, použijte operátor `delete` k uvolnění paměti, kterou jste alokovali pomocí `new`.
   - Ujistěte se, že po uvolnění paměti nebude docházet k přístupu k uvolněnému objektu.

4. **Bonus (volitelně)**:
   - Přidejte metodu pro změnu jména a věku studenta.
   - Ukažte, jak funguje alokace paměti pro pole objektů (např. pole studentů) a jak správně uvolnit paměť pro celé pole.

### Ukázka načtení jména a věku z konzole v c++:

```cpp
#include <iostream>
#include <string>

int main()
{
    std::string jmeno;
    std::cin >> jmeno; // nacte jmeno z konzole

    int vek;
    std::cin >> vek; // nacte vek z konzole
    
    return 0;
}
```
## Kritéria hodnocení:
- Správné použití operátorů `new` a `delete`.
- Kód je přehledný a dobře strukturovaný.
- Zpracování vstupu od uživatele.
- Správná správa paměti a zabránění memory leaks.
