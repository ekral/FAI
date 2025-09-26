# Úloha: Lovec pokladů v paměti

Vaším úkolem je procvičit si **ukazatelovou aritmetiku** na poli celých čísel.  
Stanete se „lovcem pokladů“, který v poli hledá **největší číslo (poklad)** a **nejmenší číslo (past)**.

Pole má 20 prvků, hodnoty jsou generovány náhodně v rozsahu 1–100.  
Vaše práce bude spočívat ve správném použití ukazatelů a jejich aritmetiky.

---

## Úkoly

1. Vytvořte ukazatel, který bude ukazovat na začátek pole.  
2. Pomocí ukazatelové aritmetiky:  
   - najděte největší číslo (**poklad**),  
   - najděte nejmenší číslo (**past**).  
3. Určete vzdálenost mezi ukazateli:  
   - nejprve v **počtu objektů typu `int`** (rozdíl ukazatelů),  
   - poté (**BONUS**) převeďte ukazatele na typ `(char*)` a spočítejte rozdíl v **bajtech**.  
4. Výsledek vypište ve formátu:

```text
Poklad nalezen: X
Past nalezena: Y
Vzdálenost mezi nimi: Z objektů typu int
Vzdálenost mezi nimi (bonus): W bajtů
```

5. (Bonus navíc) Simulujte „cestu lovce“:

   - postupně posouvejte ukazatel přes celé pole, 
   - vypisujte navštívené hodnoty,
   - a na konci oznamte, že poklad byl nalezen.


Výchozí kód:

```cpp
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define SIZE 20

int main() 
{
    int arr[SIZE];
    srand((unsigned)time(NULL));

    // naplnění pole náhodnými čísly 1–100
    for (int i = 0; i < SIZE; i++) 
    {
        arr[i] = rand() % 100 + 1;
    }

    // výpis pole pro kontrolu
    printf("Pole hodnot:\n");
    for (int i = 0; i < SIZE; i++) 
    {
        printf("%d ", arr[i]);
    }
    printf("\n");

    /*
        ÚKOL PRO STUDENTY:

        1. Vytvořte ukazatel, který bude ukazovat na začátek pole.
        2. Pomocí ukazatelové aritmetiky:
            - najděte největší číslo (poklad),
            - najděte nejmenší číslo (past).
        3. Určete vzdálenost mezi ukazateli:
            - v počtu objektů typu int,
            - (BONUS) v počtu bajtů po přetypování na (char*).
        4. Výsledek vypište ve formátu:
            Poklad nalezen: X
            Past nalezena: Y
            Vzdálenost mezi nimi: Z objektů typu int
            Vzdálenost mezi nimi (bonus): W bajtů
        5. (Bonus navíc) Simulujte „cestu lovce“.
    */

    int* p_poklad = arr;
    int* p_past = arr;

    for (int* p = arr + 1; p < arr + SIZE; p++)
    {

    }

    return 0;
}
```
