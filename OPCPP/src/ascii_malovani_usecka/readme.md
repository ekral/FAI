# Renderování úsečky

Přidejte do třídy `Platno` kód, který na plátno vykreslí úsečku. 

- Při vykreslování nezapomeňte otestovat všechny směry úsečky.
- Zvolte jakýkoliv algoritmus, nemusí být optimální, přednost má čitelnost kódu.

Naivní řešení, 
1) Máme bod A = (x1,y1) a B = (x2, y2).
2) Spočítáme (dx, dy) = B - A.
3) dmax = Max(Abs(dx), Abs(dy)).
4) stepx = dx / dmax a stepy = dy / dmax.
5) Provádíme A + (stepx, stepy) dokud je hodnota menší nebo rovna B.

Hlavičkové soubory:
```cpp
#include <stdio.h>
```

Třída `Bod2d`:

```cpp
struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{
	
	}
};
```


A třída `Platno`:

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

Odpoznámkujte zapoznámkovanou funkci a jinak kód funkce main **neměňte**.

```cpp
int main()
{
	Bod2d bodA(2.0, 3.0);
	Bod2d bodB(5.0, 6.0);

	Platno platno('-', 'x');

	bool konec = true;

	do
	{
		platno.Vymaz();
		platno.NakresliBod(2, 3);

		platno.popredi = 'O';
		platno.NakresliBod(0, 0);

		platno.popredi = '1';
		platno.NakresliBod(platno.maxColumnIndex, 0);

		platno.popredi = '2';
		platno.NakresliBod(platno.maxColumnIndex, platno.maxRowIndex);

		platno.popredi = '3';
		platno.NakresliBod(0, platno.maxRowIndex);

		platno.popredi = 'A';

		// Odpoznamkuje nasledujici radek a definujte nasledujici clenskou funkci
		//platno.NakresliUsecku(bodA, bodB);

		platno.Zobraz();

	} while (!konec);
}
```
