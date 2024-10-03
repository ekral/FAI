# Vykreslení úsečky

Máme matici znaků reprezentovanou jednorozměrným polem, kde jsou uloženy řádky ukončené znakem pro nový řádek. Díky tomu pak můžeme matici vypsat jedním příkazem.

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

#define ROZMER 4

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

    pole[k] = '\0'; // ukonceni retezce
}

int main(void)
{

    char pole[(ROZMER * (ROZMER + 1)) + 1];

    inicializuj(pole); // "----\n----\n----\n----\n"

    // vykreslete obrazec

    puts(pole);

    return 0;
}
```
