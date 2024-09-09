# 06 Třídy a destruktor

**autor: Erik Král ekral@utb.cz**

---
Pro zvládnutí testu potřebujete vědět jak se zapisuje a používá destruktor. Zatímco konstruktor se používá k inicializaci a případné alokaci zdrojů, tak deskturktor slouží naopak k uvolnění alokovaných zdrojů. Například, pokud v konstruktoru alokujeme dynamicky paměť, tak v destruktoru můžeme tuto paměť uvolnit. Destruktor se potom zavolá až přestává existovat proměnná typu instance třídy. Destruktor se jmenuje stejně jako třída ale začíná znakem `~`, nemá žádný návratový typ ani void a nemá žádné parametry.

Na následujících příkladech si probereme jednotlivé příkazy. 

* Nejprve si definujeme třídu `Pole` která si v konstruktoru alokuje paměť.

```c++
class Pole
{
public:
	int n;
	int* data;

	Pole(int n) : n(n)
	{
		data = new int[n];
	}
};
```

* Tuto alokovanou paměť můžeme uvolnit v destruktoru.

```c++
class Pole
{
public:
	int n;
	int* data;

	Pole(int n) : n(n)
	{
		data = new int[n];
	}

	~Pole()
	{
		delete[] data;
	}
};
```

* Tento desktruktor se potom v následujícím příkladu zavolá, až přestane existovat proměnná `pole`, což je na konci funkce main:

```c++
int main()
{
	Pole pole(5);
}
```

---
Následuje kompletní kód.

```c++
class Pole
{
public:
	int n;
	int* data;

	Pole(int n) : n(n)
	{
		data = new int[n];
	}

	~Pole()
	{
		delete[] data;
	}
};

int main()
{
	Pole pole(5);
}
```