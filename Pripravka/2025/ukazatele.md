# Ukazatele

## Definice

Následující definice vychází z knihy [A Tour of C++](https://www.stroustrup.com/tour3.html).

- **Typ** definuje množinu možných hodnot a množinu operací (pro objekt).
- **Hodnota** je množina bitů interpretovaná podle daného typu.
- **Objekt** je paměť, která na dané **adrese** uchovává hodnotu daného typu.
- **Proměnná** je pojmenovaný objekt.
- **Ukazatel** je proměnná která uchovává adresu objektu daného typu.

S pomocí ukazatele můžeme pracovat s jakýmkoliv objektem v paměti, pokud známe jeho adresu a typ.

## Příklad

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


---
Poznámky

- UNIX -> LINUX