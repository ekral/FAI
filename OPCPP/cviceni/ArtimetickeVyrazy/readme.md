# Úkoly na cvičení: Aritmetické operace a proměnné

## Výpočet obsahu a obvodu trojúhelníka

Máte trojúhelník definovaný delkami stran `a`,`b`, `c` a výchozí kód funkce `main`:

```cpp
#include <math.h>

int main()
{
    double a = 5.0;
    double b = 6.0;
    double c = 7.0;

    double obsah = 0.0;
    double obvod = 0.0;
 
    printf("obsah je %lf\n", obsah);
    printf("obvod je %lf\n", obvod);
}
```

Spočítejte a vypište obsah tohoto trojúhelníka pomocí Herronova vzorce:

$$\begin{align*}
s &= \frac{a + b + c}{2} \\
\text{area} &= \sqrt{s \cdot (s - a) \cdot (s - b) \cdot (s - c)}
\end{align*}$$





