# ASCII Kreslení úsečky

Přidejte do třídy `Platno` kód, který na plátno vykreslí úsečku z **bodA** do **bodB**. 

- Při vykreslování nezapomeňte otestovat všechny směry úsečky.
- Zvolte jakýkoliv algoritmus, nemusí být optimální, přednost má čitelnost kódu.
- Absolutní hodnotu zjistíme funkcí `fabs` a maximum `fmax`.

Naivní řešení:
1) Máme bod A = (x1,y1) a B = (x2, y2).
2) Spočítáme (dx, dy) = B - A.
3) dmax = fmax(fabs(dx), fabs(dy)).
4) stepx = dx / dmax a stepy = dy / dmax.
5) Provádíme A + (stepx, stepy) dokud je hodnota přibližně rovna B (například můžeme počítat od 0.0 po 1.0 do dmax).

Zdrojový kód:

```cpp
#include <iostream>
#include <string>
#include <algorithm>
#include <cmath>

using namespace std;

struct Bod2d
{
    double x;
    double y;

    Bod2d(const double x, const double y) : x(x), y(y)
    {

    }
};

class Platno
{
private:
    string retezec;
public:
    const int sirka;
    const int vyska;

    // pomoci member initializer listu volam konstruktor stringu
    Platno(const int sirka, const int vyska) : retezec((sirka + 1)* vyska, '-'), sirka(sirka), vyska(vyska)
    {
        Vymaz();
    }

    void Vymaz()
    {
        fill(retezec.begin(), retezec.end(), '-');

        for (int i = sirka; i < retezec.length(); i += sirka + 1)
        {
            retezec[i] = '\n';
        }

    }

    void Zobraz() const
    {
        cout << retezec << endl;
    }

    void NakresliBod(const double x, const double y)
    {
        if (x < 0.0 || x >= sirka || y < 0.0 || y >= vyska)
        {
            return;
        }

        const int ix = static_cast<int>(round(x));
        const int iy = static_cast<int>(round(y));

        const int pos = (vyska - iy - 1) * (sirka + 1) + ix;

        retezec[pos] = 'x';
    }
};

int main()
{
    Platno platno(3, 2);

    platno.Vymaz();

    const Bod2d A(0.0, 0.0);
    const Bod2d B(2.0, 1.0);

    platno.NakresliBod(A.x, A.y);
    platno.NakresliBod(B.x, B.y);

    //platno.NakresliUsecku(A, B); // 🚀 Implementujte

    platno.Zobraz();

    return 0;
}
```
