# 01 Opakování základů a ukazatelů
**autor: Erik Král ekral@utb.cz**

---

Pro zvládnutí prvního testu budete potřebovat znát následující pojmy:

- Co je to proměnná.
- Jak se definuje a inicializuje lokální proměnná.
- Jak zjistit adresu proměnné.
- jak zjistit počet bajtů proměnné nebo typu.
- Jaké jsou základní typy proměnných v jazyce C++.



## Základní typy 

- Typ `int` reprezenuje celé číslo se znaménkem, v jazyce C má minimálně dva bajty.
- Typ `double` reprezentuje číslo s plovoucí řádková čárka, anglicky floating-point s binárním exponentema se znaménkem. Typycky zabíra 8 bajtů.
- Typ `float` reprezentuje číslo s plovoucí řádkovou čárkou, ale s poloviční přesností proti `double`.
- Typ `bool` reprezentuje pravdu (`true`) a nepravdu (`false`).
- Typ `char` představuje jeden znak v ASCII tabulce znaků.

Pomocí adresního operátoru `&` získám adresu proměnné a pomocí operátoru `sizeof` zjistím počet bytu proměnné nebo typu.

```C
#include<stdio.h>

int main()
{
	// Definice a inicializace proměnné
	int a = 0; // cele cislo se znamenkem
	double b = 0.0; // desetinne cislo se znamenkem
	float c = 0.0f; // desetinne cislo se znamenkem s polovicnim presnosti proti double
	bool d = true;  // reprezentuje pravdu (true) a nepravdu (false)
	char e = 'a';   // jeden znak v ASCII tabulce znaku

	&a; // ziskam adresu promenne, adresni operator (operator reference)
	int pocetBytu = sizeof(a); // operator sizeof vrati pocet bytu promenne nebo typu

	putchar(97);
	putchar('a');
	putchar(7);
	putchar('\a');
	putchar('\n');

	printf("int: %d\n", a);
	printf("double: %lf\n", b);
	printf("float: %f\n", c);
	printf("bool: %d\n", d);
	printf("char: %c\n", e);
	printf("adresa: %p\n", &a);
}
```
  
Při dělení si musíme dát pozor, protože výraz ```1 / 3``` vrací hodnotu ```0``` protože oba operandy jsou celá čísla, operace dělení je v tomto případě celočíselná.
Naproti tomu výrazy ```1 / 3.0```, ```1.0 / 3``` nebo ```1.0 / 3.0``` vrací číslo s desetinnou čárkou, protože alespoň jeden z operandů je číslo s desetinnou čárkou.

```c
double x1 = 1 / 3;
double x2 = 1 / 3.0;
double x3 = 1.0 / 3;
double x4 = 1.0 / 3.0;
```

Poznámka: V jazyce C se používá desetinná tečka, protože vychází z angličtiny.

## Základní vstupně výstupní operace

Pro výpis typu ```int``` používáme formátovací značku **%d**.

```c
int x = 0;
printf("%d", x);
```

Pro výpis typu ```double``` používáme formátovací značku **%lf**.

```c
double x = 0.0;
printf("%lf", x);
```

Pro vstup z terminálu **bez ověření správnosti** vstupu můžeme použít funkci ```scanf_s```, která má jako parametr adresu proměnné do které ukládá převedenou hodnotu z řetězce dle formátovací značky.

```c
int x = 0;
scanf_s("%d", &x);
```

```c
double x = 0.0;
scanf_s("%lf", &x);
```

Příkazy ```printf``` a ```scanf_s``` jsou deklarované v hlavičkovém souboru **stdio.h**.

## Ukazatele

### Definice

Následující definice vychází z knihy [A Tour of C++](https://www.stroustrup.com/tour3.html).

- **Typ** definuje množinu možných hodnot a množinu operací (pro objekt).
- **Hodnota** je množina bitů interpretovaná podle daného typu.
- **Objekt** je paměť, která na dané **adrese** uchovává hodnotu daného typu.
- **Proměnná** je pojmenovaný objekt.
- **Ukazatel** je proměnná která uchovává adresu objektu daného typu.

S pomocí ukazatele můžeme pracovat s jakýmkoliv objektem v paměti, pokud známe jeho adresu a typ.

### Příklad

V následujícíh příkladu je použitý pseudokód nahrazující operátory textem s cílem aby byl kód lépe pochopitelný. Dále je uvedený příklad ve validním kódu jazyka C a nakonec je uvedena tabulka pro srovnání.

**Typ a operátory**

- Typ ```typ_adresa_objektu```, kde uvádíme daný typ objektu, naříklad  ```typ_adresa_int_objektu```. 
- Operátor ```adresa_promenne(název proměnné)``` vrací adresu proměnné.
- Operátor ```hodnota_na_adrese(adresa)``` vrací hodnotu objektu na dané adrese.


**Příklad v pseudokódu pro vysvětlení typů a operátorů**

```c
void vynuluj(typ_adresa_int_objektu p)
{
	hodnota_na_adrese(p) = 0;
}

int main()
{
	int x = 2;
	vynuluj(adresa_promenne(x));

	return 0;
}
```

**Reálný příklad v jazyce C**

```c
void vynuluj(int* p)
{
	*p = 0;
}

int main()
{
	int x = 2;
	vynuluj(&x);

	return 0;
}
```

**Tabulka srovnávající pseudokód s reálným zápisem**

| Pseudokód | Reálný kód | Název |
|---|---|---|
| typ_adresa_int_objektu | int* | ukazatel | 
| adresa_promenne(x) | &x | adresní operátor nebo také operátor reference |
| hodnota_na_adrese(p) | *p | operátor indirekce nebo také operátor dereference | 

