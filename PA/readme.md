Test 1 - pøíprava
---
Pro zvládnutí prvního testu potøebujete znát dva datové typy a umìt definovat promìnné tìchto typù. Nezapomeòte, že záleží i na velkých a malých písmenech a promìnná `Math.Pow` musí být napsaná správnì vèetnì velkých a malých písmen.
* `double y = 0;` desetiné èíslo se znaménkem
* `int x = 0;` celé èíslo se znaménkem

Dále byste mìli znát následující operace, které si postupnì probereme na pøíkladech. 

Nejprve si ale definujme tøi promìnné `x`, `y` a `z`:
```cs 
double x = 2.0;
double y = 3.0;
double z = 0.0;
```
* Matematické operátory souèet, rozdíl, souèin, podíl, záporná hodnota, druhá mocnina. Pro druhou mocninu používáme zápis `x * x` protože protože je to rychlejší a jednoduší než použití metody *Math.Pow*.
```cs 
z = x + y; // soucet
z = x - y; // rozdil
z = x * y; // soucin
z = x / y; // podil
z = -x; // zaporna hodnota
z = x * x; // druha mocnina
```
* Matematické operace ze tøídy *Math* pro mocninu a odmocninu
```cs 
z = Math.Pow(x, 100.0); // mocnina x^100
z = Math.Sqrt(9.0); // druha odmocnina
```
* Použití konstanty PI ze tøídy *Math*
```cs 
z = 2 * Math.PI * x; // konstanta PI
```
* Urèování priorit operátorù pomocí kulatých závorek ():
```cs 
z = x * (y + 3.0); // kulate zavorky urcuji prioritu 
```

* A nakonec zmìna hodnot promìnné samotné:
```cs 
z = z + 2.0; // zvyseni o hodnotu
z = z - 2.0; // snizeni o hodnotu
++z; // zvyseni o 1
--z; // snizeni o 1
```
---
Pro typ `int` je zápis pøedchozích operací stejný, jen používáme celoèíselné numerické konstanty. Nejvìtší rozdíl je ale v tom, že metody *Math.Pow* a *Math.Sqrt* pracují s typem `double` takže výsledek musíme explicitnì pøetypovat s pomocí zápisu `(int)`. Samotné argumenty tìchto metod jsou ale typu `double` a typ `int` lze na typ `double` pøevést implicitnì (nemusíme do kódu nic psát).
```cs 
int a = 2;
int b = 3;
b = (int)Math.Pow(a, 100.0); // mocnina x^100
b = (int)Math.Sqrt(9.0); // druha odmocnina
a = a + 2; // zvyseni o hodnotu
a = a - 2; // snizeni o hodnotu
```
---
V následujících kódech je uvedený kompletní pøíklad a øešení pøíkladù ze cvièení.