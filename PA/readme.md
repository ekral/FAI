Test 1 - p��prava
---
Pro zvl�dnut� prvn�ho testu pot�ebujete zn�t dva datov� typy a um�t definovat prom�nn� t�chto typ�. Nezapome�te, �e z�le�� i na velk�ch a mal�ch p�smenech a prom�nn� `Math.Pow` mus� b�t napsan� spr�vn� v�etn� velk�ch a mal�ch p�smen.
* `double y = 0;` desetin� ��slo se znam�nkem
* `int x = 0;` cel� ��slo se znam�nkem

D�le byste m�li zn�t n�sleduj�c� operace, kter� si postupn� probereme na p��kladech. 

Nejprve si ale definujme t�i prom�nn� `x`, `y` a `z`:
```cs 
double x = 2.0;
double y = 3.0;
double z = 0.0;
```
* Matematick� oper�tory sou�et, rozd�l, sou�in, pod�l, z�porn� hodnota, druh� mocnina. Pro druhou mocninu pou��v�me z�pis `x * x` proto�e proto�e je to rychlej�� a jednodu�� ne� pou�it� metody *Math.Pow*.
```cs 
z = x + y; // soucet
z = x - y; // rozdil
z = x * y; // soucin
z = x / y; // podil
z = -x; // zaporna hodnota
z = x * x; // druha mocnina
```
* Matematick� operace ze t��dy *Math* pro mocninu a odmocninu
```cs 
z = Math.Pow(x, 100.0); // mocnina x^100
z = Math.Sqrt(9.0); // druha odmocnina
```
* Pou�it� konstanty PI ze t��dy *Math*
```cs 
z = 2 * Math.PI * x; // konstanta PI
```
* Ur�ov�n� priorit oper�tor� pomoc� kulat�ch z�vorek ():
```cs 
z = x * (y + 3.0); // kulate zavorky urcuji prioritu 
```

* A nakonec zm�na hodnot prom�nn� samotn�:
```cs 
z = z + 2.0; // zvyseni o hodnotu
z = z - 2.0; // snizeni o hodnotu
++z; // zvyseni o 1
--z; // snizeni o 1
```
---
Pro typ `int` je z�pis p�edchoz�ch operac� stejn�, jen pou��v�me celo��seln� numerick� konstanty. Nejv�t�� rozd�l je ale v tom, �e metody *Math.Pow* a *Math.Sqrt* pracuj� s typem `double` tak�e v�sledek mus�me explicitn� p�etypovat s pomoc� z�pisu `(int)`. Samotn� argumenty t�chto metod jsou ale typu `double` a typ `int` lze na typ `double` p�ev�st implicitn� (nemus�me do k�du nic ps�t).
```cs 
int a = 2;
int b = 3;
b = (int)Math.Pow(a, 100.0); // mocnina x^100
b = (int)Math.Sqrt(9.0); // druha odmocnina
a = a + 2; // zvyseni o hodnotu
a = a - 2; // snizeni o hodnotu
```
---
V n�sleduj�c�ch k�dech je uveden� kompletn� p��klad a �e�en� p��klad� ze cvi�en�.