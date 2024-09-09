# 07 Operátory new a delete

**autor: Erik Král ekral@utb.cz**

---
Pro zvládnutí testu potřebujete vědět jak požívat operátor new pro alokaci a operátor delete pro uvolnění paměti na haldě. Operátor `new` alokuje paměť na haldě podobně jako funkce `malloc` v jazyku C a stejně tak operátor `delete` paměť uvolňuje podobně jako funkce `free`. Kromě jednoduší syntaxe je nejvěším rozdílem operátoru `new` a `delete` pro funkcím z jazyka C, že tyto operátory volají konstruktor a destruktor.

Na následujících příkladech si probereme jednotlivé příkazy. 

* Nejprve si alokujeme paměť na haldě pro jedno celé číslo typu integer pomocí zápisu `new int`. Poté si uložíme adresu na alokovanou paměť do proměnné typu ukazatele `int* p`. Dále s pomocí operátoru indirekce `*p` změníme hodnotu uloženou na adrese ukazatele `p` a nakonec paměť uvolníme pomocí zápisu `delete p`:

```c++
int main()
{
	int* p = new int;
	*p = 5;
	delete p;
}
```

* Analogicky můžeme postupovat pokud chceme na haldě alokovat pole celých čísel o rozměru `n`. Pole alokujeme zápisem new `int[n]`. K prvkům tentokrát přistupujeme pomocí operátoru indexace `p[i]` (ale mohli bychom použít i operátor indirekce, například zápis `*(p + i)`). Nakonec, pokud uvolňujeme paměť pro pole, tak musíme použít příkaz `delete[]` tedy přidat hranaté závorky.

```c++
int main()
{
	int n = 10;
	int* p = new int[n];
	
	for (int i = 0; i < n; i++)
	{
		p[i] = i;
	}
	
	delete[] p;
}
```

---
Následuje kompletní kód.

```c++
int main()
{
	int n = 10;
	int* p = new int[n];
	
	for (int i = 0; i < n; i++)
	{
		p[i] = i;
	}
	
	delete[] p;

	return 1;
}
```