# Polymorfismus a virtuální funkce

**autor Erik Král ekral@utb.cz**

---

## Polymorfismus

Polymorfismem (mnohotvarost) rozumíme statický polymorfismus jako přetěžování metod, přetěžování operátorů a dynamický polyformismus, kdy chceme za běhu programu nahrazazovat objekt jiným kompatibilním objektem. Runtime čistě objektového jazyka smalltalk dokonce umožnoval pozastavit běžící program u zákazníka, vyměnit aktuální objekt za jiný, zaktualizovat reference a pokračovat v programu ve stejném stavu v jakém jsme ho přerušili.

### Polymorfismus a statická typová kontrola

Většinou se ale obecně polymorfismem v OOP myslí dynamický polyformismus a ten si nyní probereme na příkladu. Nejprve si definujeme třídy `Pejsek` a `Kocicka`.

```c++
class Pejsek
{
public:
    void Zvuk()
    {
       puts("Haf haf");
    }
};

class Kocicka
{
public:
    void Zvuk()
    {
        puts("Mnau");
    }
};
```

Nyní bychom chtěli mít třídu Zoo, do které bychom mohli dávat jak pejsky tak kočičky. Následující kód ale není platný, to znamená, že bychom do našeho Zoo mohli dávat jen kočičkyy, ale ne zvířátka různého typu do jedné zoo.

```c++
class Zoo
{
public:
    std::vector<Kocicka*> zviratka;

    void Pridej(Kocicka* kocicka)
    {
        zviratka.push_back(kocicka);
    }
};

int main()
{
    Zoo zoo;

    Pejsek rex;
    Kocicka micka;

    zoo.Pridej(&rex); // nejde prelozit
    zoo.Pridej(&micka);
}
```

Je to proto, že v jazyce C++ **není možné změnit objekt typu `Pejsek` na objekt typu `Kocicka`**. Je to proto, že jazyk C++ používá statickou typovou kontrolu (Static typing), tedy kompilátor kontroluje typy v době překladu a vyžaduje abychom explicitně v kódu vyjádřili, že jsou vzájemně nahraditelné. Tedy že mají například stejné členské proměnné a funkce. Naproti tomu v jazyce JavaScript by to bylo možné, protože v JavaScriptu nemají proměnné pevně přiřazený typ a typová kontrola je dynamická (Dynamic typing). To znamená, že teprve až za běhu programu se v jazyce JavaScript ověří, že jak kočička, tak pejsek mají metodu `Zvuk`, někdy se tomuto postupu říká **duck typing** - tedy pokud to kváká a chodí jako kachna, tak je to kachna.

V jazyce C++, protože má statickou typovou kontrolu, vyjádříme že jsou objekty kompatibilní buď pomocí rodičovské třídy nebo pomocí rozhraní. V následujícím kódu si nadefinujeme rodičovskou třídu `Zviratko` od ktere bude dedit jak `Pejsek`, tak Kocicka:

```c++
class Zviratko
{
public:
    void Zvuk()
    {
        puts("Jsem abstraktni zviratko a nedelam zadny konkretni zvuk");
    }
};

class Pejsek : public Zviratko
{
public:
    void Zvuk()
    {
       puts("Haf haf");
    }
};

class Kocicka : public Zviratko
{
public:
    void Zvuk()
    {
        puts("Mnau");
    }
};
```

### Upcasting

Nyní můžeme prostřednictví ukazatele (nebo reference) typu `Zviratko` nahradit pejska kočičkou a naopak. Této operaci, kdy převádíme potomka na rodiče říkáme **upcasting**.

```c++
Pejsek rex;
Kocicka micka;

Zviratko* zviratko;

zviratko = &rex;
zviratko = &micka;
```

A v zoo můžeme mít seznam zvířátek, do kterého můžeme dávat pejsky, kočičky a v budoucnu i všechna nová zvířátka, pokud budou potomkem třídy `Zviratko`:

```c++
std::vector<Zviratko*> zviratka;

zviratka.push_back(&rex);
zviratka.push_back(&micka);
```

### Polymorfismus, early a late binding v OOP

V minulém příkladu jsme si vytvořili seznam zvířátek do kterého jsme přidali pejska a kočičku. Pokud ale prostřednictvím ukazatele typu `Zviratko` zavoláme funkci `Zvuk`, tak se nám zavolá metoda třídy `Zviratko` a na terminál se vypíše dvakrát text "Jsem abstraktni zviratko a nedelam zadny konkretni zvuk". Je to opět proto, že v jazyk C++ používá **static typing** a o tom, která metoda se zavolá se rozhodne *v době překladu dle typu ukazatele*. V kontextu OOP mluvíme o **early bindingu**.

```c++
for(Zviratko* zviratko : zviratka)
{
    zviratko->Zvuk();
}
```

V jazyce JavaScript, protože používá dynamic typing, se o tom, která metoda se zavolá rozhoduje až za běhu programu. Proto by se zavolali správně metody pejska a kočičky. Což je to co chceme. V kontextu OOP tomu říkáme **late binding**. Pokud je late bindig očekáváné chování, proč se v jazyce C++ a nebo jazyce C# nepoužívá jako výchozí? Nepoužívá se jako výchozí z důvodu výkonu, protože rozhodování o tom, která metoda se má zavolat až za běhu programu je pomalejší, než když se o tom rozhodne je jednou hned při překladu programu.

V jazyce C++ a dalších z důvodu výkonu explicitně říkáme aby používali pomalejší late bindig jen ty metody u kterých to potřebujeme. V našem příkladu označíme metodu `Zvuk` v třídě `Zviratko` jako `virtual` a třídách `Pejsek` a `Kocicka` ji označíme klíčovým slovem `override`. Říkáme, že překrýváme virtuální metodu. Tímto zápisem potomu určíme, že se má pro metodu `Zvuk` použít late binding, tedy o tom, která metoda se zavolá se rozhodne až **za běhu programu dle typu objektu**.

V následujícím kompletním příkladu máme překrytou vrituální metodu `Zvuk` a zvířátka v seznamu zvířátek už správně vypisují konkrétní zvuky, které dělají:

```c++
#include <cstdio>
#include <vector>

class Zviratko
{
public:
    virtual ~Zviratko() = default; // virtualni destruktor, zatim bez tela

    virtual void Zvuk()
    {
        puts("Jsem abstraktni zviratko a nedelam zadny konkretni zvuk");
    }
};

class Pejsek : public Zviratko
{
public:
    void Zvuk() override
    {
       puts("Haf haf");
    }
};

class Kocicka : public Zviratko
{
public:
    void Zvuk() override
    {
        puts("Mnau");
    }
};

int main()
{
    Pejsek rex;
    Kocicka micka;

    std::vector<Zviratko*> zviratka;
    zviratka.push_back(&rex);
    zviratka.push_back(&micka);

    for(Zviratko* zviratko : zviratka)
    {
        zviratko->Zvuk();
    }
}
```

### Downcasting

Operaci, kdy přetypujeme potomka na rodiče říkáme upcasting. Vyjímečně ale můžeme i v kódu provést downcasting, kdy ale musíme být opatrní, protože ne každé zvířátko může být například kočička. Využíváme především příkaz `dynamic_cast<Kocicka*>(zviratko)` který používáme u tříd, které mají virtuální funkce:

```c++
for(Zviratko* zviratko : zviratka)
{
    Kocicka* k = dynamic_cast<Kocicka*>(zviratko);

    if(k != NULL)
    {
        puts("Je to kocicka");
    }
}
```
## Pure virtual functions

Pokud virtuální funkce nemá žádné smysluplné tělo, tak můžeme použít pure virtual function. Třída, která obsahuje nebo zdědí pure virtual function je abstraktní třída a nemůžeme vytvářet její instance. Můžou být pouze ukazatele a reference na tuto třídu.

```c++
#include <cstdio>
#include <vector>

class Zviratko
{
public:
    virtual ~Zviratko() = default; 

    virtual void Zvuk() = 0; // pure virtual function
};

class Pejsek : public Zviratko
{
public:
    void Zvuk() override
    {
        puts("Haf haf");
    }
};

class Kocicka : public Zviratko
{
public:
    void Zvuk() override
    {
        puts("Mnau");
    }
};

int main()
{
    
    Pejsek rex;
    Kocicka micka;

    // Zviratko zviratko; // nejde prelozit
    Zviratko* ukazatel = &rex; // jde prelozit
    Zviratko& reference = rex; // jde prelozit

    std::vector<Zviratko*> zviratka;
    zviratka.push_back(&rex);
    zviratka.push_back(&micka);

    for(Zviratko* zviratko : zviratka)
    {
        zviratko->Zvuk();
    }
}
```

## Virtuální funkce a destruktor

Pokud třída obsahuje virtuální funkci, tak musí být i destruktor virtuální, tak aby se zavolal správný destruktor při upcastingu. Kdyby by v následujícím příkladu nebyl destruktor třídy `Rodic` virtuální, tak by se nezavolal.

```c++
#include <cstdio>
#include <vector>

class Rodic
{
public:
    virtual ~Rodic()
    {
        puts("Destructor rodice");
    }
};

class Potomek final : public Rodic
{
public:
    ~Potomek() override
    {
        puts("Destructor potomka");
    }
};

int main()
{
    const Rodic* rodic = new Potomek();
    delete rodic;
}
```

---
Důležité je si uvědomit, že výše zmíněné postupy se týkají především statically typed jazyků se zaměřením na výkon. Ve Smalltalku, který je dynamically typed, nebylo potřeba definovat virtuální funkce, protože všechny funkce byly jako výchozí late bind a nebylo nutné definovat rozhraní nebo rodičovskou třídu kvůli kompatibilitě objektů. Dá se říct, že OOP bylo ve smalltalku mnohem jednodušší a většina syntaxe kterou se teď učíme pochází z implementace OOP ve statically typed jazyce. 