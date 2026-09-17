# Python vs. C11 a C++23: Quick Reference Sheet

Tento přehled pomáhá při rychlém přepnutí z Pythonu do C a C++. Pro běžné procedurální ukázky používá **C11**. Tam, kde je vhodné moderní objektové nebo API pro práci s kolekcemi, používá **C++23**.

Pozor na středníky, složené závorky, ruční správu paměti v C a nutnost překládat C++ kód kompilátorem C++.

---

## 1. Základní syntaxe

| Vlastnost | Python | C11 / C++23 |
| :--- | :--- | :--- |
| Ukončení příkazu | nový řádek | středník `;` |
| Bloky kódu | odsazení | složené závorky `{ }` |
| Logické operátory | `and`, `or`, `not` | `&&`, `\|\|`, `!` |
| Komentáře | `# komentář` | `//` nebo `/* ... */` |
| Výstup na konzoli | `print(x)` | C11: `printf("%d\n", x);`, C++23: `std::println("{}", x);` |

---

## 2. Proměnné a typy (C11)

C i C++ jsou staticky typované. C11 nemá `string`: řetězec je pole znaků ukončené nulovým znakem, typicky `char *` nebo `char[]`.

```c
#include <stdio.h>

// Python: x = 10
int x = 10;

// Python: name = "Alice"
const char* name = "Alice";

// Python: pi = 3.14
double pi = 3.14;

printf("%s: %d\n", name, x);
```

> `const char* name = "Alice"` ukazuje na nemenny literal. Pro upravitelny text pouzij `char name[] = "Alice";`.


## 3. Podmínky a cykly (C11)

Podmínka je vždy v kulatých závorkách. Nula znamená nepravdu, nenulová hodnota pravdu.

### If-else

Python:

```python
if x > 0:
    print("Kladne")
elif x < 0:
    print("Zaporne")
else:
    print("Nula")
```

C11:

```c
if (x > 0)
{
    printf("Kladne\n");
}
else if (x < 0)
{
    printf("Zaporne\n");
}
else
{
    printf("Nula\n");
}
```

### For a while

```c
// Python:
// for i in range(5):
//     print(i)
for (int i = 0; i < 5; i++)
{
    printf("%d\n", i);
}

// Python:
// while x > 0:
//     x -= 1
while (x > 0)
{
    x--;
}
```

---

## 4. Pole a dynamická paměť (C11)

Pole v C má pevnou velikost. Jeho délku je nutné si předat nebo spočítat v místě, kde je skutečné pole ještě dostupné.

```c
#include <stddef.h>
#include <stdlib.h>

// Python: values = [1, 2, 3]
int values[] = { 1, 2, 3 };
size_t count = sizeof values / sizeof values[0];

// Python: values = [0] * 3
int zeros[3] = { 0 };

// Python: dynamic_values = [0] * 3
size_t dynamic_count = 3;
int *dynamic_values = malloc(dynamic_count * sizeof *dynamic_values);

if (dynamic_values != NULL)
{
    dynamic_values[0] = 10;
}

free(dynamic_values);
```

> Každé úspěšné `malloc` musí mít odpovídající `free`. V C++ pro dynamická data obvykle použij `std::vector`, ne `new[]`.

---

## 5. Funkce (C11)

Funkce musí mít návratový typ. Pokud nic nevrací, použij `void`.

```c
#include <stdio.h>

// Python:
// def pozdrav(jmeno):
//     print(f"Ahoj {jmeno}")
void pozdrav(const char *jmeno)
{
    printf("Ahoj %s\n", jmeno);
}

// Python:
// def scitej(a, b):
//     return a + b
int scitej(int a, int b)
{
    return a + b;
}
```

---

## 6. Třídy a instance (C++23)

Třídy nejsou součástí C11, proto zde používáme C++23. `std::string` vlastní svůj text a `this` odpovídá Pythonímu `self`.

```cpp
#include <print>
#include <string>
#include <utility>

class Student
{
public:
    std::string jmeno;
    int body = 0;

    explicit Student(std::string jmeno)
        : jmeno(std::move(jmeno))
    {
    }

    void pozdrav() const
    {
        std::println("Ahoj {}", jmeno);
    }
};
```

Instance tříd:

```cpp
// Python: pavel = Student("Pavel")
Student pavel{"Pavel"};

// Python:
// karel = Student("Karel")
// karel.body = 40
Student karel{"Karel"};
karel.body = 40;
```

---

## 7. Dynamické pole a foreach (C++23)

`std::vector` je běžná C++ náhrada Python listu. Range-based `for` je moderní podoba `foreach`.

```cpp
#include <cstddef>
#include <print>
#include <vector>

// Python: values = [1, 2, 3]
std::vector<int> values{1, 2, 3};

// Python: values.append(4)
values.push_back(4);

// Python: len(values)
std::size_t count = values.size();

// Python:
// for value in values:
//     print(value)
for (int value : values)
{
    std::println("{}", value);
}
```

Pro změnu prvku iteruj referenci:

```cpp
for (int& value : values)
{
    value *= 2;
}
```

---

## 8. List comprehension a ranges (C++23)

C++ nemá vestavěnou list comprehension, ale C++23 ranges umí podobně skládat filtry a transformace. Pro uložení výsledku do `std::vector` použij `std::ranges::to`.

```cpp
#include <ranges>
#include <vector>

std::vector<int> data{1, 4, 6, 9, 12};

// Python: filtered = [x for x in data if x > 5]
auto filtered = data
    | std::views::filter([](int value) { return value > 5; })
    | std::ranges::to<std::vector>();

// Python: squares = [x * x for x in data]
auto squares = data
    | std::views::transform([](int value) { return value * value; })
    | std::ranges::to<std::vector>();
```

---

## 9. None a výsledek s chybou (C++23)

Pro nulový ukazatel použij `nullptr`. Když operace může vrátit hodnotu nebo chybu, použij `std::expected<T, E>`.

```cpp
#include <expected>
#include <print>
#include <string>

std::expected<int, std::string> nacti_body(bool je_platne)
{
    if (!je_platne)
    {
        return std::unexpected("Neplatny vstup");
    }

    return 42;
}

auto score = nacti_body(false);
if (!score.has_value())
{
    std::println("Chyba: {}", score.error());
}
```

---

## Rychlá pomoc při chybách

1. Chybí `;` na konci příkazu.
2. C a C++ rozlišují velká a malá písmena.
3. V C je `'a'` jeden znak (`char`) a `"abc"` řetězec ukončený `\0`.
4. C kód překládej jako C11, C++ kód jako C++23. Nemíchej `printf` a `std::println` bez důvodu.

---

Odkazy:

- [C reference](https://en.cppreference.com/w/c)
- [C++ reference](https://en.cppreference.com/w/cpp)
- [C++ ranges](https://en.cppreference.com/w/cpp/ranges)
