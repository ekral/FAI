# 02 Třídy a členské proměnné
**autor: Erik Král ekral@utb.cz**

---

Třídu definujeme pomocí klíčového slova `class`. Pokud chceme přistupovat k členským proměnným mimo třídu, tak je musíme deklarovat jako `public`. Následující příklad definuje třídu pro dvourozměrný bod s public členskými proměnnými `x` a `y`. Na rozdíl od ostatních jazyků, například Java, nebo C# musíme v jazyce C++ uvést za definicí třídy středník `;`.

```c++
class Bod
{
public:
	int x;
	int y;
};
```

* Proměnnou typu třída definujeme stejným způsobem jako zabudované typy. V následujícím příkladu definujeme proměnnou `b1` typu `Bod`.
```c++
Bod b1;
```
* K členským proměnným potom přistupujeme pomocí operátoru přímého přístupu `.`. 
```c++
Bod b1;
b1.x = 2;
b1.y = 3;

printf("Bod b1 ma souradnice x: %d y: %d", b1.x, b1.y);
```
* Stejně jako u zabudovaných typů se při přiřazení hodnoty kopíruje hodnota v paměti. Relační, aritmetické a další operátory bychom ale museli sami definovat.

```c++
Bod b1;
b1.x = 2;
b1.y = 3;

Bod b2 = b1;
```

---
Následují kompletní příklady.

```c++
#include <stdlib.h>

// definice vlastniho typu
struct Bod2d
{
    // clenske promenne, kde se alokuji ?
    double x;
    double y;
};

// globalni promenna, alokuji se ve statickem bloku
int y = 0;
Bod2d bod1; 

int main()
{
    // lokalni promenna, alokuji se na zasobniku
    int x = 0;
    Bod2d bod2;
    bod2.x = 2.0; // operator primeho pristupu k clenskym prvkum
    bod2.y = 3.0;
  
    Bod2d* pBod = (Bod2d*)malloc(sizeof(Bod2d));

    free(pBod);
}
```

```c++
#include <stdio.h>

struct Obdelnik
{
public:
	int a;
	int b;

	int obvod()
	{
		return 2 * (a + b);
	}

	/*Obdelnik() : a(1), b(1)
	{
	}*/

	Obdelnik(int a, int b) : a(a), b(b)
	{

	}
};

int main()
{
	Obdelnik o1(2, 3);

	Obdelnik o2(4, 5);


	Obdelnik pole[] = { o1, o2, Obdelnik(5, 6) };

	for (Obdelnik obdelnik : pole)
	{
		int obvod = obdelnik.obvod();
		printf("a: %d b: %d obdelnik: %d\n", obdelnik.a, obdelnik.b, obvod);
	}
}
```