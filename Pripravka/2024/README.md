# Letní programátorská přípravka 2024


## Online reference

- Online reference pro jazyky C a C++: [cppreference.com](https://en.cppreference.com/w/).

## Přihlašovací údaje do PC v učebnách

login: **.\student**

heslo: **student**

Poznámka: znak \ je vedle levé klávesy shift.

## Přihlašovací údaje k WIFI

Název wifi sítě: **PRIPRAVKA**

heslo: **programatorskaPripravka2024**

## Vývojová prostředí

- [Erik Král](https://fai.utb.cz/contacts/ing-et-ing-erik-kral-ph-d/) používá [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/).
- [Tomáš Dulík](https://fai.utb.cz/contacts/ing-tomas-dulik-ph-d/) používá [Clion](https://www.jetbrains.com/clion/).

## Organizace kurzu

- 9:00 První blog výuky:
	- první hodina,
	- přestávka na kávu 10-15 minut,
	- druhá hodina.
- 11:00 Obědová přestávka (60 minut).
- 12:00 Druhý blog výuky:
	- třetí hodina,
	- přestávka na kávu 10-15 minut,
	- čtvrtá hodina.
- 14:00 Konec.

## Probraná témata

1. Pondělí
	- Historie jazyka C.
	- Seznámení s vývojovým nástrojem.
	- Program hello world.
	- Typy ```int``` a ```double```.
	- Definice proměnné, adresa a velikost proměnné.
	- Aritmetické výrazy.
	- Druhá mocnina.
	- Symbolická konstanta ```M_PI```.
	- Funkce pro druhou odmocninu ```sqrt```.
	- Funkce pro mocninu ```pow```.

	Příklady:	
	- Adresa a délka proměnné [01_prvni](src/01_prvni.c).
	- Výpočet bmi [02_bmi](src/02_bmi.c).
	- Průměr tří čísel [03_prumer_tri_cisel.c](src/03_prumer_tri_cisel.c).
	- Obsah a obvod čtverce [04_ctverec.c](src/04_ctverec.c).
	- Obsah a obvod kruhu [05_kruh.c](src/05_kruh.c).
	- Obsah a obvod trojúhelníka [05_trojuhelnik.c](src/05_trojuhelnik.c).
	- Splátka hypotéky [06_splatka.c](src/06_splatka.c). 


2. Úterý
	- Opakování aritmetických výrazů.
	- Booleanovské výrazy (relační operátory, operátor rovnosti, logické operátory).
	- Podmíněný příkaz.

 	Příklady
	- Výpočet diskriminantu [07_diskriminant.c](src/07_diskriminant.c).
 	- Výpis konstanty M_PI [08_vypis_pi.c](src/08_vypis_pi.c)
  	- Druhá mocnina a odmocnina [09_druha_mocnina_odmocnina.c](src/09_druha_mocnina_odmocnina.c).
   	- N-tá mocnina [10_nta_mocnina.c](src/10_nta_mocnina.c).
   	- Kořeny kvadratické rovnice [11_koreny.c](src/11_koreny.c).
   	- Bmi slovní kategorie [12_bmi_kategorie.c](src/12_bmi_kategorie.c).
   	- Existuje trojúhelník a je pravoúhlý [13_pravouhly_trojuhelnik.c](src/13_pravouhly_trojuhelnik.c).
   
## Základní typy a aritmetické operace

- Typ ```int``` reprezenuje celé číslo se znaménkem, v jazyce C má minimálně dva bajty.
- Typ ```double``` reprezentuje číslo s desetinnou čárkou (spíše plovoucí řádková čárka, anglicky floating-point, typicky s binárním exponentem) a se znaménkem. Typycky zabíra 8 bajtů.
  
Výraz ```1 / 3``` vrací hodnotu ```0``` protože oba operandy jsou celá čísla, operace dělení je v tomto případě celočíselná.
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

## Proměnná

### Definice

Následující definice vychází z knihy [A Tour of C++](https://www.stroustrup.com/tour3.html).

- **Typ** definuje množinu možných hodnot a množinu operací (pro objekt).
- **Hodnota** je množina bitů interpretovaná podle daného typu.
- **Objekt** je paměť, která na dané **adrese** uchovává hodnotu daného typu.
- **Proměnná** je pojmenovaný objekt.
- **Ukazatel** je proměnná která uchovává adresu objektu daného typu.

S pomocí ukazatele můžeme pracovat s jakýmkoliv objektem v paměti, pokud známe jeho adresu a typ.



