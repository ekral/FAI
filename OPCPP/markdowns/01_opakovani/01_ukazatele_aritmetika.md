# Ukazatele aritmetika

**autor: Erik Král**

---
Pro zvládnutí testu potřebujete vědět jak pracovat s ukazateli, jak funguje ukazatelová aritmetika a co je to operátor `sizeof`.

Ukazatel je naržený pro práci s objekty v paměti a často se používá pro práci s polem stejných objektů v paměti. Pokud máme například ukazatel `p` který ukazuje na nějaký objekt v paměti, tak výraz `++p` posune ukazatel na další objekt v paměti. Pokud máme například ukazatel `int* p`, tak pokud tento ukazatel zvýšíme pomocí výrazu `++p`, tak se ukazatel posune na další objekt typu `int` v paměti. Adresa kam ukazatel ukazuje se tedy musí zvýšit o kolik bajtů kolik zabírá objekt typu `int` v paměti. Ke zjištění kolik zabírá typ nebo proměnná v paměti bajtů slouží operátor `sizeof`, například výraz `sizeof(int)` vrací počet bajtů kolik zabírá v paměti typ `int`. 

* V následujícím příkladu definujeme ukazatel `int* p` tedy ukazatel na `int` a přiřadíme mu jako adresu číslo `0`. Ukazateli nelze přidělit implicitně jako adresu numerické typy, ale `0` je výjímka. Často ji přiřazujeme ukazateli abychom označili, že ukazatel zatím nemá přiřazenou použitelnou adresu. V jazyce C můžeme místo hodnoty `0` použít makro `NULL` a v `C++` používáme klíčové slovo `nullptr`. V následujícím příkladu ale adresu `0` použijeme k tomu, abychom jednoduše zjistili o kolik se změní adresa ukazatele pokud inkrementujeme ukazatel. V příkladu nejprve výpišeme hodnotu `sizeof(int)` což bude většinou 4 bajty. Pokud zvýšíme hodnotu ukazatele pomocí výrazu `++p` tak se adresa v ukazateli zvýší o hodnotu `sizeof(int)` tedy o `4 bajty`.

```c++
printf("%d\n", sizeof(int)); // int zabere v paměti 4 bajty

int* p = 0;
printf("%p\n", p); // 00000000

++p; // zvýší adresu o 4 bajty
printf("%p\n", p); // 00000004

++p; // zvýší adresu o 4 bajty
printf("%p\n", p); // 00000008
```

* Pokud máme ukazatel `double* p` tedy ukazatel na typ `double`. Tak se v následujícím příkladu zvýší adresa o hodnotu `sizeof(double)` tedy o 8 bajtů. Adresa je zapsaná v šestnáctkové (hexadecimální soustavě).

```c++
printf("%d\n", sizeof(double)); // double zabere v paměti 8 bajtů

double* p = 0;
printf("%p\n", p); // 00000000

++p; // zvýší adresu o 8 bajtů
printf("%p\n", p); // 00000008

++p; // zvýší adresu o 8 bajtů
printf("%p\n", p); // 00000010 (16 desítkově)
```

* U ukazatele můžeme kromě operátoru inkremetace použít také operátory `--`, `+`, `-`, `+=` a `-=`, které v následujícím příkladu vždy změní adresu kam ukazatel ukazuje o násobky `sizeof(int)`

```c++
printf("%d\n", sizeof(int)); // 4

int* p = 0;
printf("%p\n", p); // 00000000

p = p + 2; // zvýší adresu o 2 x 4 = 8 bajtů
printf("%p\n", p); // 00000008

--p; // sníží adresu o 4 bajty
printf("%p\n", p); // 00000004

p += 3; // zvýší adresu o 3 x 4 = 12 bajtů
printf("%p\n", p); // 00000010

p -= 4; // sníží adresu o 4 x 4 = 16 bajtů
printf("%p\n", p); // 00000000
```

* Ukazatele můžeme od sebe i odečítat, v tomto případě je výsledek opět v počtech objektů, například typu `int`. V následujícím příkladu máme ukazatele na `int` `p1` a `p2`. Ukazatel `p2` posuneme o dva objekty příkazem `p2 += 2;`. Adresa kam ukazuje ukazatel `p2` se zvýší o *2 x `sizeof(int)`* tedy o *2 x 4 = 8* bajtů a adresa kam ukazuje ukazatel `p1` zůstala `0`. Rozdíl mezi ukazatelem `p2 - p1` ale bude logicky `2` a ne `8`, protože rozdíl je opět v počtech objektů `int`, stejně jako celá ukazatelová aritmetika.

```c++
printf("%d\n", sizeof(int)); // 4

int* p1 = 0;
int* p2 = 0;

p2 += 2;

int rozdil = p2 - p1;

printf("%d\n", rozdil); // 2

printf("%p\n", p1); // 00000000
printf("%p\n", p2); // 00000008
```

---
Následuje kompletní kód.

```c++
#include <stdio.h>

int main()
{
	printf("%d\n", sizeof(int)); // 4

	int* p = 0;
	printf("%p\n", p); // 00000000

	p = p + 2; // zvýší adresu o 2 x 4 = 8 bajtů
	printf("%p\n", p); // 00000008

	--p; // sníží adresu o 4 bajty
	printf("%p\n", p); // 00000004

	p += 3; // zvýší adresu o 3 x 4 = 12 bajtů
	printf("%p\n", p); // 00000010  (16 desítkově)

	p -= 4; // sníží adresu o 4 x 4 = 16 bajtů
	printf("%p\n", p); // 00000000
}
```

```c++
#include <stdio.h>

int main()
{
	printf("%d\n", sizeof(int)); // 4

	int* p1 = 0;
	int* p2 = 0;

	p2 += 2;

	int rozdil = p2 - p1;

	printf("%d\n", rozdil); // 2
	
	printf("%p\n", p1); // 00000000
	printf("%p\n", p2); // 00000008
}
```