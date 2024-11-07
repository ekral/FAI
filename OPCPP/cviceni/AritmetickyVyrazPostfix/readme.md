# Úloha: Program pro vyhodnocení postfixového výrazu pomocí zásobníku

Napište program, který pomocí zásobníku vyhodnotí aritmetický výraz v postfixové notaci. Tento typ zápisu výrazů se používá k eliminaci závorek a operátorů, takže operátory jsou vždy aplikovány na předchozí dvě hodnoty.

Příklad:

Vstupní výraz: 5 3 + 8 2 / *

Vysvětlení:

1. Nejprve se provede 5 3 +, což znamená součet 5 a 3, tedy 8.
2. Poté se provede 8 2 /, což znamená dělení 8 děleno 2, tedy 4.
3. Nakonec se provede 8 * 4, což znamená násobení 8 a 4, tedy 32.
Výstup: 32

Řešení:
1. Budeme používat zásobník pro ukládání operandů.
2. Když narazíme na operand (číslo), vložíme ho na zásobník.
3. Když narazíme na operátor (+, -, *, /), vybereme dva operandy ze zásobníku, provedeme operaci a výsledek vrátíme zpět na zásobník.
4. Na konci by měl být na zásobníku jediný prvek, což je výsledek výrazu.

Použijte následující zásobník. 

```cpp
#include <assert.h>
#include <algorithm>
class Stack
{
private:
    int n;
    int* data;
    int pos;
public:
    explicit Stack(const int n) : n(n), data(new int[n]), pos(0)
    {
    }

    // kopirovaci konstruktor
    // typ reference
    Stack(const Stack& other) : n(other.n), data(new int[n]), pos(0)
    {
        std::copy_n(other.data, n, data);
    }

    void operator = (const Stack& other) = delete;

    ~Stack()
    {
        delete[] data;
    }

    void push(const int x)
    {
        assert(pos < n);

        data[pos] = x;
        ++pos;
    }

    int pop()
    {
        assert(pos > 0);

        --pos;
        return data[pos];
    }

    int operator[] (const int i) const
    {
        return data[i];
    }

    int count() const
    {
        return pos;
    }
};

// pouzity typ reference
void print(const Stack& stack)
{
    for(int i = 0; i < stack.count(); ++i)
    {
        std::cout << stack[i] << std::endl;
    }
}
```
---
Vlastní typ pro zásobník používáme jen pro potřeby cvičení. V reálném kód bysme použili už hotový typ [std::stack](https://en.cppreference.com/w/cpp/container/stack).