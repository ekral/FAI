# Úkol prohození čísel

Máme následující kód, doplňte kód tak, aby kód nespadnul díky příkazu assert. 

- Využijte:
    - ukazatele,
    - adresní operátor,
    - operátor indirekce.
    - 
-Kód funkce **main neměňte**.

```cpp
#include <assert.h>
#include <stdio.h>

void prohod(int x, int y)
{
    int tmp = x;
    x = y;
    y = tmp;
}

int main()
{
    int x = 2;
    int y = 3;

    prohod(x, y);

    printf("x: %d y: %d\n", x, y);

    assert(x == 3);
    assert(y == 2);
}
```


