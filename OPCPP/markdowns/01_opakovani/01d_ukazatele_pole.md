# Ukazatele a pole

**autor: Erik Král ekral@utb.cz**

---
Pro zvládnutí testu potřebujete vědět jaký je vztah mezi ukazatelem a polem a jak používat operátor indexace `[]` a operátor indirekce `*` spolu s ukazatelovou aritmetikou u polí a ukazatelů.

Pole a ukazatel jsou v jazyce C a C++ prakticky dvojčata. Pole dokonce předáváme jako ukazatel funkcím. Také operátor indexace `[]` u pole, například výraz `pole[1]` překladač převede na na zápis s operátorem indexace `*(pole + 1)`. Detaily si probereme v následujících příkladech.

* Nejprve si definujeme pole o 4 prvcích, inicializujeme všechny jeho prvky na hodnotu 0 a potom změníme hodnoty prvků pomocí operátoru indexace `[]`.

```c++
int pole[4] = { 0 };

pole[0] = 1;
pole[1] = 2;
pole[2] = 3;
pole[3] = 4;
```

* Nyní si stejný příklad přepíšeme s použitím ukazatele. Definujeme si tedy ukazatel `int* p` a pomocí výrazu `&pole[0]` mu přiřadíme jako hodnotu adresu prvního prvku poli. Analogicky, výraz `&pole[1]` by vrátil adresu druhého prvku v poli.

```c++
int* p = &pole[0];
```

* S ukazateli můžeme používat operátor indexace `[]` a přistupovat s jeho pomocí k objektům v paměti, v tomto případě prvkům v poli:

```c++
p[0] = 1;
p[1] = 2;
p[2] = 3;
p[3] = 4;
```

* K prvům ale můžeme přistupovat i pomocí operátoru indirekce `*` a ukazatelové aritmetiky, například zápis `*(p + 1)` představuje druhý prvek. Tomuto zápisu dáváme u ukazatelů přednost před použitím operátoru indexace.

```c++
*(p + 0) = 1;
*(p + 1) = 2;
*(p + 2) = 3;
*(p + 3) = 4;
```

* Analogicky ale můžeme použít operátor indirekce `*` u pole. Oba zápisy jsou tedy vzájemně zaměnitelné, ale u polí dáváme přednost operátoru indexace `[]`.

```c++
*(pole + 0) = 1;
*(pole + 1) = 2;
*(pole + 2) = 3;
*(pole + 3) = 4;
```

* Ukazatel a pole jsou si dokonce tak podobné, že s využitím explicitního přetypování můžeme převést pole přímo na ukazatel. Jediný rozdíl mezi ukazatelem a polem je, že pole není proměnná a nemůžeme měnit jeho hodnotu, jen hodnoty jednotlivých proměnných.

```c++
int* p = (int*)pole;
```

* Jako poslední příklad si uvedeme rychlejší přístup k prvkům pole pomocí operátoru inkremetace `++` u ukazatele. Pokud přistupujeme k prvkům pole jeden po druhém (někdy říkáme lineárně), tak můžeme zápis zoptimalizovat a zapsat následujícím způsobem, kdy změníme objekt na který ukazatel ukazuje a potom posuneme ukazatel na další objekt. Po poslední operaci bude ukazatel ukazovat na poslední prvek v poli.

```c++
*p = 17;
++p;
*p = 18;
++p;
*p = 19;
++p;
*p = 20;
```

---
Následuje kompletní kód, všimněte si, že v něm předáváme funkci `vypis` pole jako ukazatel.

```c++
#include <stdio.h>

void vypis(int* pole, int n)
{
	printf("vypis pole:\n");

	for (int i = 0; i < n; i++)
	{
		printf("%d ", pole[i]);
	}

	putchar('\n');
}

int main()
{
	constexpr int n = 4; // konstanta známá v době překladu

	int pole[n] = { 0 };

	vypis(pole, n);

	pole[0] = 1;
	pole[1] = 2;
	pole[2] = 3;
	pole[3] = 4;

	vypis(pole, n);

	*(pole + 0) = 5;
	*(pole + 1) = 6;
	*(pole + 2) = 7;
	*(pole + 3) = 8;

	vypis(pole, n);

	int* p = &pole[0];
	// int* p = (int*)pole;

	p[0] = 9;
	p[1] = 10;
	p[2] = 11;
	p[3] = 12;

	vypis(pole, n);

	*(p + 0) = 13;
	*(p + 1) = 14;
	*(p + 2) = 15;
	*(p + 3) = 16;

	vypis(pole, n);

	*p = 17;
	++p;
	*p = 18;
	++p;
	*p = 19;
	++p;
	*p = 20;

	// na tomto radku ukazuje ukazatel p na posledni prvek v poli

	vypis(pole, n);
}
```