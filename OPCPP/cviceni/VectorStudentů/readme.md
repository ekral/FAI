# Úkol: Práce s operátory `new` a `delete` a dynamické pole `vector` v C++

## Cíl:
Cílem úkolu je pochopit a implementovat použití operátorů `new` a `delete` v jazyce C++, přičemž se zaměříme na dynamickou alokaci paměti pomocí kontejneru `std::vector`.

## Zadání:
1. **Vytvořte třídu**:
   - Název třídy: `Student`
   - Atributy: 
     - `std::string name` (jméno studenta)
     - `int age` (věk studenta)
   - Metody:
     - Konstruktor s parametry pro inicializaci jména a věku.
     - Destruktor, který vypíše zprávu o zničení objektu.
     - Metoda pro výpis informací o studentovi.

2. **Dynamická alokace**:
   - Vytvořte program, který:
     - Používá `std::vector<Student*>` pro ukládání instancí studentů.
     - Nabízí uživatelské menu s následujícími možnostmi:
       1. Přidat nového studenta
       2. Vypsat všechny studenty
       3. Ukončit program
   
3. **Uvolnění paměti**:
   - Při přidání nového studenta, alokujte paměť pomocí `new` a přidejte instanci do vektoru.
   - Při ukončení programu (před jeho ukončením) uvolněte všechny instance studentů pomocí `delete`, abyste předešli memory leaks.

4. **Bonus (volitelně)**:
   - Přidejte možnost pro změnu jména a věku studenta v menu.
   - Umožněte uživateli vymazat konkrétního studenta ze seznamu.

## Výchozí kód

```cpp
#include <iostream>
#include <string>
#include <vector>

class Student
{
    std::string name;
    std::string age;

};
int main()
{
    std::string jmeno;
    std::cin >> jmeno; // nacte jmeno z konzole

    int vek;
    std::cin >> vek; // nacte vek z konzole

    std::vector<Student*> students;
    Student* student = new Student;

    students.push_back(student);

    delete students[0];
    
    return 0;
}
```

## Kritéria hodnocení:
- Správné použití operátorů `new` a `delete`.
- Použití `std::vector` pro dynamické ukládání studentů.
- Kód je přehledný a dobře strukturovaný.
- Zpracování vstupu od uživatele a správná správa paměti.
