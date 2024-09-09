04 Třídy a konstruktor

**autor: Erik Král ekral@utb.cz**

---
Pro zvládnutí testu potřebujete vědět jak definovat a používat konstruktory. 

Na následujících příkladech si probereme jednotlivé příkazy. 

*  Nejprve si definujeme třídu `Ctverec`:

```c++
class Ctverec
{
public:
	double n;
};

int main()
{
	Ctverec c1;
	c1.n = 0.0;
	printf("Ctverec ma delku strany %lf", c1.n);
}
```

* V předchozím příkladu jsme pro nastavení hodnoty poloměru použili zápis `k1.n = 0.0`. Pro inicializaci hodnot členských proměnných ale můžeme použít i speciální funkci které se říká konstruktor. Tato funkce nemá žádný návratový typ, ani `void` a jmenuje se stejně jako třída, v našem případě tedy `Ctverec`

```c++ 
class Ctverec
{
public:
	double n;

	Ctverec()
	{
		n = 0.0;
	}
};
```

* Když teď vytvoříme instanci třídy `Ctverec`, tak se automaticky zavolá konstruktor a nastaví hondotu proměnné `n` na `0`.

```c++ 
Ctverec c1; // n bude mit hodnotu 0.0
```

* Konstruktor může mít i parametry, v následujícím příkladu máme konstruktor s parametrem `n`:

```c++ 
class Ctverec
{
public:
	double n;

	Ctverec(double n)
	{
		this->n = n;
	}
};
```

* Konstruktoru potom předáme argument při vytváření instance třídy v kulatých závorkách: 

```c++ 
Ctverec c1(3.0); // n bude mit hodnotu 3.0
```

* Konstruktor může být, stejně jako všechny funkce v c++, přetížený. To znamená, že můžeme mít funkci se stejným názvem, pokud se liší v počtu nebo typu parametrů. V jedné třídě tedy můžeme tedy mít konstruktor jak s parametrem tak bez parametru.

```c++ 
class Ctverec
{
public:
	double n;

	Ctverec()
	{
		n = 0;
	}

	Ctverec(double n)
	{
		this->n = n;
	}
};
```

* A oba konstruktory potom můžeme použít podle potřeby:

```c++ 
Ctverec c1;		// n bude mit hodnotu 0.0
Ctverec c2(3.0);	// n bude mit hodnotu 3.0
```

---
Následuje kompletní kód.

```c++
#include <stdio.h>

class Ctverec
{
public:
	double n;

	Ctverec()
	{
		n = 0;
	}

	Ctverec(double n)
	{
		this->n = n;
	}
};

int main()
{
	Ctverec c1;		// n bude mit hodnotu 0.0
	Ctverec c2(3.0);	// n bude mit hodnotu 3.0
	printf("Ctverec ma polomer %lf\n", c1.n);
	printf("Ctverec ma polomer %lf\n", c2.n);

	return 1;
}
```