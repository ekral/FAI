# 05 Konstruktor member inicializer list

**autor: Erik Král ekral@utb.cz**

---
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