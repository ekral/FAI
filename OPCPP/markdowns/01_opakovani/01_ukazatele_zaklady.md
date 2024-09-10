# 01 Ukazatelé základy

**autor: ekral@utb.cz**

---

Pro zvládnutí testu potřebujete vědět jak definovat typ ukazatel, a používat adresní operátor  a operátor indirekce.

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

### Definice ukazatele

* Typ ukazatel je odvozený od jiných typů a zapisuje se přidáním znaku `*` za název typu, například typ `int*` je ukazatel na celé číslo a typ `double*` je ukazatel na číslo s desetinou čárkou. Způsob definice ukazatele je navržen jako mnemotechnická pomůcka, který nám v následujícím příkladu u ukazatele `p` napovídá, že výraz `*p` je typu `int`, což si vysvětlíme dále.

```c++
int *p;
```

* Na jednom řádku můžeme definovat i více proměnných, v následujícím kódu definujeme proměnou `x` typu `int` a proměnou `y` typu `int*` tedy ukazatel. Opět zde v zápisu vidíme nápovědu, že výrazy `x` a `*y` jsou typu `int`.

```c++
int x, *y;
```

* Pozice znaku `*` při definice ukazatele ale není povinná a můžeme jej zapsat ihned za typ nebo až před název proměné, případně mít mezery před i za znakem `*`. Všechny tři následující zápisy mají stejný význam. Můžeme si zvolit jeden ze způsobů a ten potom dodržovat, případně dodržovat doporučený styl pro projekt na kterém pracujeme.

```c++
int *x;
int* y;
int * z;
```

* Pozor ale na následující zápis, kdy ukazatelem by byla pouze proměná `x` a proměnná `y` by byla obyčejný typ `int`:

```c++
int *x, y;
```

* pokud chceme aby obě proměnné byly typu ukazatel, tak musíme znak `*` uvést u obou proměných:

```c++
int *x, *y;
```

### Adresní operátor

* Proměnná je pojmenovaná hodnota v paměti uložená na konkrétní adrese. Adresní operátor `&` (nazývaný také jako operátor reference - pozor neplést s typem reference v C++) vrací adresu proměnné v paměti. Například výraz `&x` představuje adresu proměnné `x` v paměti. V následujícím příkladu definujeme ukazatel `p` kterému přiřazujeme jako hodnotu adresu proměnné `x` v paměti.

```c++
int x = 2;
int* p = &x;
```

### Operátor indirekce

Pokud pracujeme přímo s ukazatelem, tak pracujeme s jeho hodnotou což je adresa. V následujícím příkladu definujeme ukazatel `p1` kterému přiřadíme adresu proměnné `x` a potom definujeme ukazatel `p2` které přiřadíme hodnotu ukazatele `p1`. Oba ukazatele tedy mají jako hodnotu adresu proměnné `x`.

```c++
int x = 2;
int* p1 = &x;
int* p2 = p1;
```

Pokud chceme ale pracovat s hodnotou uloženou na adrese na kterou ukazatel ukazuje, tak musíme použít operátor indirekce (také nazývaný operátor dereference) `*` před proměnou typu ukazatel. Pokud máme ukazatel typu `int` například `int* p;` tak výraz `*p` můžeme použít všude tam, kde můžeme použít proměnnou typu `int`. V následujícím příkladu zvyšujeme hodnotu na kterou ukazuje ukazatel `p` o `1`. 
  * Příkazy `*p = *p + 1;` a `*p += 1;` zvyšují hodnotu proměnné na kterou ukazuje ukazatel, protože operátor indirekce se vyhodnocuje dříve než aritmetické operátory.
  * Výraz s využitím prefix operátoru inkrementace `++*p` je také v pořádku, protože se nejprve provede indirekce a potom operátor inkrementace. Výrazy s unárními operátory jako je indirekce `*` a inkrementace `++` se totiž vyhodnocují zprava doleva.
  * U výrazu s postfix operátorem inkrementace `(*p)++` musíme použít závorky protože, u výrazu `*p++` by se nejprve zvýšila adresa ukazatele `p` a teprve potom se použil operátor indirekce. Opět platí, že výrazy s unárními operátory jako je indirekce `*` a inkrementace `++` se vyhodnocují zprava doleva.

```c++
int x = 2;
int* p = &x;

*p = 0;
*p = *p + 1;
*p += 1;
++*p;
(*p)++;
```


---
Následuje kompletní kód.

```c++
#include <iostream>

int main()
{
	int x = 2;
	int* p = &x;

	*p = 0;
	*p = *p + 1;
	*p += 1;
	++*p;
	(*p)++; // musime pouzit zavorky, protoze unarni operatory jako * a ++ se vyhodnocuje zprava doleva a bez zavorek by nejdrive zvysil hodnotu ukazatele a pak teprve provedl indirekci
}
```