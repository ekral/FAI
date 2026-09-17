# Python vs. C11 a C++23: Quick Reference Sheet

Tento prehled pomaha pri rychlem prepnuti z Pythonu do C a C++. Pro bezne proceduralni ukazky pouziva **C11**. Tam, kde je vhodne moderni objektove nebo kolekcni API, pouziva **C++23**.

Pozor na stredniky, slozene zavorky, rucni spravu pameti v C a nutnost prekladat C++ kod kompilatorem C++.

---

## 1. Zakladni syntaxe

| Vlastnost | Python | C11 / C++23 |
| :--- | :--- | :--- |
| Ukonceni prikazu | novy radek | strednik `;` |
| Bloky kodu | odsazeni | slozene zavorky `{ }` |
| Logicke operatory | `and`, `or`, `not` | `&&`, `\|\|`, `!` |
| Komentare | `# komentar` | `//` nebo `/* ... */` |
| Vystup na konzoli | `print(x)` | C11: `printf("%d\n", x);`, C++23: `std::println("{}", x);` |

---

## 2. Promenne a typy (C11)

C i C++ jsou staticky typovane. C11 nema `string`: retezec je pole znaku ukoncene nulovym znakem, typicky `char *` nebo `char[]`.

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

---

## 3. Podminky a cykly (C11)

Podminka je vzdy v kulatych zavorkach. Nula znamena nepravdu, nenulova hodnota pravdu.

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

## 4. Pole a dynamicka pamet (C11)

Pole v C ma pevnou velikost. Jeho delku je nutne si predat nebo spocitat v miste, kde je skutecne pole jeste dostupne.

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

> Kazde uspesne `malloc` musi mit odpovidajici `free`. V C++ pro dynamicka data obvykle pouzij `std::vector`, ne `new[]`.

---

## 5. Funkce (C11)

Funkce musi mit navratovy typ. Pokud nic nevraci, pouzij `void`.

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

## 6. Tridy a instance (C++23)

Tridy nejsou soucasti C11, proto zde pouzivame C++23. `std::string` vlastni svuj text a `this` odpovida Pythonimu `self`.

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

Instance trid:

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

## 7. Dynamicke pole a foreach (C++23)

`std::vector` je bezna C++ nahrada Python listu. Range-based `for` je moderni podoba `foreach`.

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

Pro zmenu prvku iteruj referenci:

```cpp
for (int& value : values)
{
    value *= 2;
}
```

---

## 8. List comprehension a ranges (C++23)

C++ nema vestavenou list comprehension, ale C++23 ranges umi podobne skladat filtry a transformace. Pro ulozeni vysledku do `std::vector` pouzij `std::ranges::to`.

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

## 9. None a vysledek s chybou (C++23)

Pro nulovy ukazatel pouzij `nullptr`. Kdyz operace muze vratit hodnotu nebo chybu, pouzij `std::expected<T, E>`.

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

## Rychla pomoc pri chybach

1. Chybi `;` na konci prikazu.
2. C a C++ rozlisuji velka a mala pismena.
3. V C je `'a'` jeden znak (`char`) a `"abc"` retezec ukonceny `\0`.
4. C kod prekladej jako C11, C++ kod jako C++23. Nemichej `printf` a `std::println` bez duvodu.

---

Odkazy:

- [C reference](https://en.cppreference.com/w/c)
- [C++ reference](https://en.cppreference.com/w/cpp)
- [C++ ranges](https://en.cppreference.com/w/cpp/ranges)
