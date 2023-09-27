# Tema 1

Pro zvládnutí prvního testu budete potřebovat znát následující pojmy:

- Co je to proměnná.
- Jak se definuje a inicializuje lokální proměnná.
- Jak zjistit adresu proměnné.
- jak zjistit počet bajtů proměnné nebo typu.
- Jaké jsou základní typy proměnných v jazyce C++.

```C
#include<stdio.h>

int main()
{
	// Proměnná je pojmenovana hodnota v pameti
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
Pojmy jsou vysvětleny v [souboru main_zaklady.cpp](main_zaklady.cpp).

