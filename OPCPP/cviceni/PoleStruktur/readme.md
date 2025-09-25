# Pole struktur

Máme pole studentů, kdy každý student psal dva testy. Najděte a vypište jméno studenta, který měl z prvního nebo druhého testu nejvíce bodů.

```cpp
#include <stdio.h>

#define MAX(a,b) (((a) > (b)) ? (a) : (b))

struct Student
{
    const char* jmeno;
    double test1;
    double test2;
};

void vypis(struct Student* studenti, int pocet)
{
    for (int i = 0; i < pocet; i++)
    {
        printf("%s test1: %lf test2: %lf\n", studenti[i].jmeno, studenti[i].test1, studenti[i].test2);
    }
}

int main(void)
{
    double vetsi = MAX(10, 5);

    struct Student studenti[] =
    {
        {"Karel", 70.0, 50.0},
        {"Jiri", 30.0, 50.0},
        {"Alena", 70.0, 90.0},
        {"Pavel", 50.0, 50.0}
    };

    vypis(studenti, 4);

    // Najdete a vypiste jmeno studenta s nejlepsim vysledkem ze dvou testu

    return 0;
}
```
