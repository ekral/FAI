# Úkoly na ukazatele

## 1) Prohození čísel

Máme následující kód, změňte kód tak, aby kód šel přeložit a nespadnul díky příkazu assert. Kód funkce **main neměňte**.

- Najděte v kódu adresní operátor (také se mu říká operátor reference).
- Využijte a komentářem označte:
    - typ ukazatel,
    - operátor indirekce.

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

    prohod(&x, &y);

    printf("x: %d y: %d\n", x, y);

    assert(x == 3);
    assert(y == 2);
}
```


## 1) Funkce pro výpis pole

Definujte a zavolejte funkci `vypis`, která vypíše prvky pole na terminal. Využijte ukazatel.


```cpp
#include <stdio.h>

int main(void)
{
    int pole[] = { 7, 6, 4, 2, 7 };
    const int delka =  sizeof(pole) / sizeof(int);

    return 0;
}
```