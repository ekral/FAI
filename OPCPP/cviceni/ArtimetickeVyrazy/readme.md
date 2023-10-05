# Úkoly na cvičení: Aritmetické operace a proměnné

## Výpočet obsahu a obvodu trojúhelníka

Máte trojúhelník definovaný delkami stran `a`,`b`, `c` a výchozí kód funkce `main`:

```cpp
#include <stdio.h>
#define _USE_MATH_DEFINES
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

## Výpočet splátky hypotéky

Spočítejte výši měsíční spátky hypotéky dle vzorce:

$$M = \frac{P \cdot r \cdot (1 + r)^n}{(1 + r)^n - 1}$$

Kde:
*M* je měsíční splátka hypotéky.
*P* je výše půjčené částky (počáteční zůstatek půjčky).
*r* je měsíční úroková sazba (roční úroková sazba dělená 12 měsíci a převedená na desetinné číslo).
*n* je celkový počet měsíčních plateb (doba trvání půjčky v letech násobená 12.




