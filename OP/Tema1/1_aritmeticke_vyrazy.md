Test 1 - příprava
---
Pro zvládnutí předmětu potřebujete znát dva datové typy a umět definovat proměnné těchto typů. Nezapomeňte, že záleží i na velkých a malých písmenech a proměnná `Math.Pow` musí být napsaná správně včetně velkých a malých písmen.
* `double y = 0;` desetiné číslo se znaménkem
* `int x = 0;` celé číslo se znaménkem

Dále byste měli znát následující operace, které si postupně probereme na příkladech. 

Nejprve si ale definujme tři proměnné `x`, `y` a `z`:
```cs 
double x = 2.0;
double y = 3.0;
double z = 0.0;
```
* Matematické operátory součet, rozdíl, součin, podíl, záporná hodnota, druhá mocnina. Pro druhou mocninu používáme zápis `x * x` protože protože je to rychlejší a jednoduší než použití metody *Math.Pow*.
```cs 
z = x + y; // soucet
z = x - y; // rozdil
z = x * y; // soucin
z = x / y; // podil
z = -x; // zaporna hodnota
z = x * x; // druha mocnina
```
* Matematické operace ze třídy *Math* pro mocninu a odmocninu
```cs 
z = Math.Pow(x, 100.0); // mocnina x^100
z = Math.Sqrt(9.0); // druha odmocnina
```
* Použití konstanty PI ze třídy *Math*
```cs 
z = 2 * Math.PI * x; // konstanta PI
```
* Určování priorit operátorů pomocí kulatých závorek ():
```cs 
z = x * (y + 3.0); // kulate zavorky urcuji prioritu 
```

* A nakonec změna hodnot proměnné samotné:
```cs 
z = z + 2.0; // zvyseni o hodnotu
z = z - 2.0; // snizeni o hodnotu
++z; // zvyseni o 1
--z; // snizeni o 1
```
---
Pro typ `int` je zápis předchozích operací stejný, jen používáme celočíselné numerické konstanty. Největší rozdíl je ale v tom, že metody *Math.Pow* a *Math.Sqrt* pracují s typem `double` takže výsledek musíme explicitně přetypovat s pomocí zápisu `(int)`. Samotné argumenty těchto metod jsou ale typu `double` a typ `int` lze na typ `double` převést implicitně (nemusíme do kódu nic psát).
```cs 
int a = 2;
int b = 3;
b = (int)Math.Pow(a, 100.0); // mocnina x^100
b = (int)Math.Sqrt(9.0); // druha odmocnina
a = a + 2; // zvyseni o hodnotu
a = a - 2; // snizeni o hodnotu
```
---
V následujících kódech je uvedený kompletní příklad a řešení příkladů ze cvičení.
