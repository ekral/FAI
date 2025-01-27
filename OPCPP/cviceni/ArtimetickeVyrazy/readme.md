# Úkoly na cvičení: Aritmetické operace a proměnné

## Výpočet obsahu a obvodu trojúhelníka

Máte trojúhelník definovaný delkami stran `a`,`b`, `c`. 

Spočítejte a vypište obsah tohoto trojúhelníka pomocí Herronova vzorce:

$$\begin{align*}
s &= \frac{a + b + c}{2} \\
\text{area} &= \sqrt{s \cdot (s - a) \cdot (s - b) \cdot (s - c)}
\end{align*}$$

Výchozí kód funkce **main**:

```cpp
#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

int main()
{
    double a = 5.0;
    double b = 6.0;
    double c = 7.0;

    double obvod = 0.0;
    double obsah = 0.0;
    
    printf("obsah je %lf\n", obsah);
    printf("obvod je %lf\n", obvod);
}
```

## Výpočet splátky hypotéky

Spočítejte a vypište výši měsíční spátky hypotéky dle vzorce:

$$m = \frac{p \cdot r \cdot (1 + r)^n}{(1 + r)^n - 1}$$

Kde:
- *m* je měsíční splátka hypotéky.
- *p* je výše půjčené částky (počáteční zůstatek půjčky).
- *r* je měsíční desetinná úroková míra (roční úroková míra v procentech (6%) dělená 12 měsíci (6 / 12) a převedená na desetinné číslo ( 6 / 12 / 100).
- *n* je celkový počet měsíčních plateb (doba trvání půjčky v letech násobená 12).

Pojmy:
- **Úrok**: nominální částka, například 5000,- Kč.
- **Úroková míra**:  úrok vyjádřený v procentech z částky, například 6 %.
- **Desetinná úroková míra**: úrok vyjádřený jako desetinné číslo pro výpočet v matematických operacích, například 0.06.

Dále vypište splátkový kalendář. Každý měsíc vypište výši úroku, úmoru a aktuálního dluhu:

$n-krát$ zopakujte následující kroky:
1) Nejprve spočítejte nominální výši úroku, tedy $urok = r \cdot p$
2) Úmor se potom rovná výše splátky - nominální výše úroku, tedy $umor = m - urok$
3) Snižte částku *p* o výši úmoru, tedy $p = p - umor$.
   
Výchozí kód funkce **main**:

```cpp
#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

int main()
{
    double p = 6000000.0;
    double rocniUrokProcenta = 6.0;
    int pocetLet = 30;

    double r = 0.0;
    int n = 0.0;

    double m = 0.0;

    printf("splatka je %lf\n", m); // 35973.03 
}
```


