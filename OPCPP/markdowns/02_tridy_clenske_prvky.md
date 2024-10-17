# 02 Třídy a členské prvky

**autor: Erik Král ekral@utb.cz**

---

## Členské proměnné

Třídu definujeme pomocí klíčového slova `class`. Pokud chceme přistupovat k členským proměnným mimo třídu, tak je musíme deklarovat jako `public`. Rozdíl mezi třídou a strukturou je v tom, že struktura má výchozí modifikátor `public` a třída má výchozí modifikátor `private`, což znamená, že pokud ve třídě neuvedeme žádný modifikátor, tak se použije `private`. 

Následující příklad definuje třídu pro dvourozměrný bod s public členskými proměnnými `x` a `y`. Na rozdíl od ostatních jazyků, například Java, nebo C# musíme v jazyce C++ uvést za definicí třídy středník `;`.

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

## Členské funkce

Pro zvládnutí testu potřebujete vědět jak definovat a používat členské funkce. 

Na následujících příkladech si probereme jednotlivé příkazy. 

*  Nejprve si definujeme třídu `Ctverec` pomocí klíčového slova `class`. Připomínám, že pokud chceme přistupovat k členským prvkům (členským proměnným i členským funkcím) mimo třídu, tak je musíme deklarovat jako `public` a za definicí třídy musí být uvedený středník `;`.

```c++
class Ctverec
{
public:
	double n;
};

int main()
{
	Ctverec k1;
	k1.n = 3.0;
	printf("Ctverec ma delku strany %lf", k1.n);
}
```

* Nyní si do třídy Ctverec přidáme funkci `Obsah`, která vrátí obsah čtverce. Každá funkce má jeden návratový typ, v tomto případě vracíme obsah jakou typ `double` a proto je hlavička funkce `double Obsah()`. Nemusíme předávat žádné parametry, protože členská proměnná `n` bude mít ve funkci takovou hodnotu jako v instanci třídy pro kterou funkci voláme.

```c++
class Ctverec
{
public:
	double n;

	double Obsah()
	{
		return n * n;
	}
};
```

* Funkci potom zavoláme pomocí jejího názvu `k1.Obsah()` a návratovou hodnotu si můžeme přiřadit do jiné proměnné nebo ji přímo použít. Dejte si pozor abyste nezapoměli uvést kulaté závorky, protože zápis `k1.Obsah` by šel také přeložit, ale vrátil by ukazatel na funkci.

```c++ 
double obsah = k1.Obsah();
printf("Ctverec ma delku strany %f a obsah %f\n", k1.n, obsah);
// nebo
printf("Ctverec ma delku strany %f a obsah %f\n", k1.n, k1.Obsah());
```

* Funkce může mít i parametry, například v následujícím příkladu máme funkci `ZmenPolomer`, která změní hodnotu členské proměnné `n` na novou hodnotu. Funkce, která nevrací žádnou hodnotu má návratový typ `void`.

```c++ 
class Ctverec
{
public:
	double n;

	double Obsah()
	{
		return n * n;
	}

	void ZmenPolomer(double novaHodnota)
	{
		n = novaHodnota;
	}
};
```

* Této funkci potom předáváme argument v kulatých závorkách, například `k1.ZmenPolomer(3.0)`:

```c++
Ctverec k1;
k1.ZmenPolomer(3.0);
```
* Parametr funkce může mít stejný název jako čleská proměnná, ale musíme je potom odlišit pomocí klíčového slovat `this` :

```c++ 
void ZmenPolomer(double n)
{
	this->n = n;
}
```

* Funkce může mít více parametrů které uvádíme v seznamu odděleném čárkou, například v následujícím příkladu máme funkci `ZmenRozmery` třídy `Obdelnik`, kde máme dva parametry:

```c++ 
class Obdelnik
{
public:
	double a;
	double b;

	double Obsah()
	{
		return a * b;
	}
	void ZmenRozmery(double a, double b)
	{
		this->a = a;
		this->b = b;
	}
};
```

* Této funkci potom předáme dva argumenty zápisem `o1.ZmenRozmery(2.0, 3.0)`:

```c++ 
Obdelnik o1;
o1.ZmenRozmery(2.0, 3.0);
printf("Obdelnik ma rozmery a: %f b: %f a obsah %f\n", o1.a, o1.b, o1.Obsah());
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

class Ctverec
{
public:
	double n;

	double Obsah()
	{
		return n * n;
	}

	double Obvod()
	{
		return 4 * n;
	}

	void ZmenPolomer(double n)
	{
		this->n = n;
	}
};

int main()
{
	Ctverec k1;
	k1.n = 3.0;
	double obsah = k1.Obsah();
	printf("Ctverec ma delku strany %lf a obsah %lf\n", k1.n, obsah);
	printf("Ctverec ma obvod %lf\n", k1.Obvod());
	
	return 1;
}
```


```c++
#include <stdio.h>

class Obdelnik
{
public:
	double a;
	double b;

	double Obsah()
	{
		return a * b;
	}

	double Obvod()
	{
		return 2 * (a + b);
	}
	void ZmenRozmery(double a, double b)
	{
		this->a = a;
		this->b = b;
	}
};

int main()
{
	Obdelnik o1;
	o1.ZmenRozmery(2.0, 3.0);
	printf("Obdelnik ma rozmery a: %lf b: %lf a obsah %lf\n", o1.a, o1.b, o1.Obsah());
	printf("Obdelnik ma obvod %lf\n", o1.Obvod());
	
	return 1;
}
```
