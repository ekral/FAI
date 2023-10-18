# Použití typu std::vector pro data ASCII plátna

Místo pole s pevnou délkou `char data[totalChars]` použijte v třídě `Platno` dynamické pole `std::vector` a změňte třídu tak, aby bylo možné zadávat rozměry plátna v konstruktoru.

```cpp
int columnCount = 30;
int rowCount = 20;
Platno platno(columnCount, rowCount, '-', 'x');
```

Zdrojový kód třídy plátno:

```cpp
class Platno
{
private:
    // static constexpr je moderni zpusob zadani konstanty zname v dobe prekladu
    static constexpr int columnCount = 30;
    static constexpr int rowCount = 20;
    static constexpr int totalChars = columnCount * rowCount;
    char pozadi;

    char data[totalChars];
public:
    static constexpr int maxColumnIndex = columnCount - 1;
    static constexpr int maxRowIndex = rowCount - 1;

    char popredi;

    Platno(char pozadi, char popredi) : pozadi(pozadi), popredi(popredi), data{ 0 }
    {
        Vymaz();
    }

    void Vymaz()
    {
        for (int i = 0; i < totalChars; i++)
        {
            data[i] = pozadi;
        }
    }

    void NakresliBod(double x, double y)
    {
        int pos = ((rowCount - round(y) - 1) * columnCount) + round(x);

        data[pos] = popredi;
    }

    void NakresliUsecku(Bod2d bodA, Bod2d bodB)
    {
        double dx = bodB.x - bodA.x;
        double dy = bodB.y - bodA.y;


        double dmax = fmax(fabs(dx), fabs(dy));

        double stepx = dx / dmax;
        double stepy = dy / dmax;

        Bod2d bod = bodA;

        double d = 0;

        while (d <= dmax)
        {
            NakresliBod(bod.x, bod.y);

            bod.x += stepx;
            bod.y += stepy;

            ++d;
        }

    }

    void Zobraz()
    {
        int pos = 0;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                char znak = data[pos];
                ++pos;

                putchar(znak);
            }

            putchar('\n');
        }
    }

};
```