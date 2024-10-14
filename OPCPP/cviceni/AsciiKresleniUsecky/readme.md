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
#include <sstream>
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
    string data;
public:
    const int sirka;
    const int vyska;


    Platno(const int sirka, const int vyska) : data((sirka + 1) * vyska, '-'), sirka(sirka), vyska(vyska)
    {
        Vymaz();
    }

    void Vymaz()
    {
        ranges::fill(data, '-');

        for (int i = sirka; i < data.length(); i += sirka + 1)
        {
            data[i] = '\n';
        }

    }

    void Zobraz() const
    {
        cout << data << endl;
    }

    void Zapis(double x, double y)
    {
        const int pos = static_cast<int>(round(y * (sirka + 1) + x));

        data[pos] = 'x';
    }
};

int main()
{
    Platno platno(20, 10);

    platno.Vymaz();
    platno.Zapis(9, 4);

    Bod2d A(2.0, 3.0);
    Bod2d B(7.0, 9.0);

    platno.NakresliUsecku(A, B); // 🚀 Implementujte

    platno.Zobraz();

    return 0;
}
```
