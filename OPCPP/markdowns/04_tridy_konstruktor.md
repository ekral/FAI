# 04 Třída, konstruktor a member intializer list 

**autor: Erik Král ekral@utb.cz**

---

## Konstruktor

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

## Member initializer list

Pro zvládnutí testu potřebujete vědět jak pužívat konstruktor member inicializer list, který se může použít pro všechny členské proměnné a musí se použít pro inicializaci hodnot konstantních členských proměnných, referencí a instancí tříd bez default konstruktoru (konstruktoru bez parametrů).

Na následujících příkladech si probereme jednotlivé příkazy. 

* Nejprve si definujeme třídu Obdelnik s výchozím konstruktorem, ve kterém inicializujeme hodnoty členský proměnných `m` a `n` a hodnotu `0.0`:

```c++
class Obdelnik
{
public:
	double m;
	double n;

	Obdelnik() 
	{
		m = 0.0;
		n = 0.0;
	}
};
```

* Zápis `m = 0.0;` můžeme nahradit následujícím zápisem:

```c++
class Obdelnik
{
public:
	double m;
	double n;

	Obdelnik() : m(0.0)
	{
		n = 0.0;
	}
};
```

* A analogicky můžeme nahradit i inicializaci členské proměnné `n`:
```c++

class Obdelnik
{
public:
	double m;
	double n;

	Obdelnik() : m(0.0), n(0.0)
	{

	}
};
```

*  Stejným způsobem můžeme nahradit i následující inicializaci v konstruktoru s parametry `m` a `n`:

```c++
class Obdelnik
{
public:
	double m;
	double n;

	Obdelnik(double m, double n)
	{
		this->m = m;
		this->n = n;
	}
};
```

* s využitím member inicializer list vypadá inicializace takto:

```c++
class Obdelnik
{
public:
	double m;
	double n;

	Obdelnik(double m, double n) : m(m), n(n)
	{

	}
};
```

* Tento zápis musíme ho použít pokud třída nemá výchozí konstruktor bez parametrů. Mějme následující definici třídy `Souradnice` která má pouze konstrutor s parametry a nemá default konstruktor bez parametrů.

```c++ 
class Souradnice
{
public:
	double x;
	double y;

	Souradnice(double x, double y) : x(x), y(y)
	{

	}
};
```

* Tuto třídou `Souradnice` potom chceme použít v třídě `Obdelnik`. Pokud bychom nepoužili member initializer list, tak by kód nešel ani přeložit, protože třída `Souradnice` nemá výchozí (default) konstruktor.

```c++ 
class Obdelnik
{
public:
	double m;
	double n;
	Souradnice stred;

	Obdelnik(double m, double n, double x, double y) : m(m), n(n), stred(x, y)
	{

	}
};
```

---
Následuje kompletní kód včetně ukázky inicializace konstatní členské proměnné

```c++
#include <stdio.h>

class Grafika
{

};

class Souradnice
{
public:
	double x;
	double y;

	Souradnice(double x, double y) : x(x), y(y)
	{

	}
};

class Obdelnik
{
public:
	const int id;
	double m;
	double n;
	Souradnice stred;

	Obdelnik(int id, double m, double n, double x, double y) : id(id), m(m), n(n), stred(x, y)
	{

	}
};

int main()
{
	Obdelnik c1(1, 2.0, 3.0, 100, 200);	
	printf("id %d: %lf x %lf, x: %lf, y: %lf", c1.id, c1.m, c1.n, c1.stred.x, c1.stred.y); 

	return 1;
}
```