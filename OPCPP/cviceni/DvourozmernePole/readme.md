# Vykreslení úsečky

Máme matici znaků reprezentvoanou jednorozměrným polem, kde jsou uloženy řádky ukončené znakem pro nový řádek. Díky tomu pak můžeme matici vypsat jedním příkazem.

Doplnňte kód, který do matice vykreslí následující obrazce:

1) Horní a dolní řádek

```
xxxxxxxxxx
----------
----------
----------
----------
----------
----------
----------
----------
xxxxxxxxxx
```

2) Levý a pravý sloupec

```
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
```

3) Úhlopříčky

```
x--------x
-x------x-
--x----x--
---x--x---
----xx----
----xx----
---x--x---
--x----x--
-x------x-
x--------x
```

4) Trojúhleník

```
----xx-----
----xx----
---x--x---
---x--x---
--x----x--
--x----x--
-x------x-
-x------x-
x--------x
xxxxxxxxxx
```

```c
#include <stdio.h>

#define ROZMER 10

void inicializuj(char* pole)
{
    int k = 0;
    for (int i = 0; i < ROZMER; i++)
    {
        for (int j = 0; j < ROZMER; j++)
        {
            pole[k] = '-';
            ++k;
        }

        pole[k] = '\n';
        ++k;
    }
}

int main(void)
{
    char pole[ROZMER * (ROZMER + 1)];

    inicializuj(pole);

    // vykreslete obrazec

    puts(pole);

    return 0;
}
```
