# 08 Dedičnost kódu a kompozice

**autor Erik Král ekral@utb.cz**

---

Pomocí dědičnosti kódu se může stát součástí tříd kód jiné třídy. Dědičnost kódu slouží k tomu, bychom nemuseli opakovat stejný kód ve více třídách a mohli ho jen zdědit.

Třída od které dědíme je (synonyma):
- Základní třída (base class).
- Rodičovská třída (parent class).
- Nadtřída (superclass).

Třída, která dědí je (synonyma):
- Odvozená třída (derived class).
- Potomek třídy (child class).
- Podtřída (subclass).

Říkáme, že odvozená třída dědí od základní třídy. Ale také můžeme říct:
- Odvozená třída je specializací  (Specialization) základní třídy.
- Základní třída je zobecněním (Generalization) odvozené třídy.
- Odvozená třída rozšířuje (Extends) základní třídu.

V následujícím příkladu máme rodičovskou třídu Osoba. Od třídy osoba potom dědí dva potomci, třídy `Student` a `Ucitel`. Součástí tříd `Student` a `Ucitel` se stane kód třídy `Osoba`.

```c++
class Osoba
{
public:
    int id;
    string jmeno;
};

class Student : public Osoba
{
public:
    int rocnik;
};

class Ucitel : public Osoba
{
public:
    string kancelar;
};

int main()
{
    Student s1;
    s1.id = 1;
    s1.jmeno = "Jiri";
    s1.rocnik = 2;

    Ucitel u1;
    u1.id = 2;
    u1.jmeno = "Tomas";
    u1.kancelar = "A508";

    return 0;
}
```

## Konstruktory a dědičnost

- Třída potomka volá konstruktor rodiče pomocí názvu třídy v member initializer listu.
- Pokud má rodič a potomek jen konstruktor s parametry, tak potomek musí zavolat konstruktor rodiče.

```c++
class Osoba
{
public:
    int id;
    string jmeno;

    Osoba(int id, string jmeno) : id(id), jmeno(jmeno)
    {
    }
};

class Student : public Osoba
{
public:
    int rocnik;

    Student(int id, string jmeno, int rocnik) : Osoba(id, jmeno), rocnik(rocnik)
    {
    }
};

int main()
{
    Student student(1, "Jiri", 2);

    return 0;
}
```

## Modifikátor přístupu protected

- Modifikátor přístupu `protected` se používá pouze v souvislosti s dědičností.
- K protected členským prvkům může vývojář přistupovat v třídě potomka, ale stejně jako u `private` k nim nemůže přistupovat mimo třídu rodiče nebo potomka.

```c++
class Rodic
{
protected:
    int x;
};

class Potomek : public Rodic
{
public:
    int Vrat()
    {
        return x;
    }
};

int main()
{
    Rodic r;
    r.x = 3; // nepujde prelozit

    Potomek p;
    p.x = 3; // nepujde prelozit

    int t = p.Vrat();

    return 0;
}
```

## Kompozice

Pokud jeden objekt zahrnuje druhý objekt, tak můžeme mluvit o vztahu HAS-A, tedy že jeden objekt má druhý objekt. V následujícím příkladu bude mít instance třídy `Motorka` dvě instance třídy `Kola` jako členské proměnné. Protože objekt motorka objekty kola nesdílí s jinými objekty jde o kompozici. V tomto případě mluvíme o vlastnictví objektu, kdy vlastnictví objektu (ownership) v tomto kontextu znamená, že když zanikne objekt, tak s ním zaniknou i objekty, které vlastní.

```cpp
class Kolo
{
public:
    int prumer;

    Kolo(int prumer) : prumer(prumer)
    {
    }
};

class Motorka
{
private:
    Kolo predni;
    Kolo zadni;

    Motorka() : predni(20), zadni(19)
    {
    }
};
```

Pokud by objekt sdílel zahrnutý objekt s jinými objekty, tak by šlo o **agregaci**. U agregace mluvíme o tom, že objekt používá jiný objekt ale nevlastní ho. V následujícím příkladu sdílí `smsSender` více objednávek. Pojmy agregace a kompozice vycházejí z jazyka UML.

```cpp
#include <iostream>
#include <string>
#include <format>
#include <print> // c++23

using namespace std;

class SmsSender
{
public:

    void PosliSms(string text)
    {
        println("Posilam sms: {}", text);
    }
};

class Objednavka
{
private:
    SmsSender* sender;

public:

    int id;

    Objednavka(int id, SmsSender* sender) : sender(sender), id(id)
    {
    }

    void Odeslat()
    {
        sender->PosliSms(format("Objednavka {} odeslana", id));
    }
};

int main()
{
    SmsSender smsSender;

    Objednavka objednavka1(1, &smsSender);
    Objednavka objednavka2(2, &smsSender);

    objednavka1.Odeslat();
    objednavka2.Odeslat();
}
```

## Dědičnost vs kompozice

Dědičnost kódu můžeme nahradit do určité míry kompozicí. V následujícím příkladu nepoužíváme dědičnost, ale třída `Student` si vytváří vlastní instanci třídy `Osoba`. Všimněte si, že mezdi třídami Osoba a Student pořád platí vztah, že Student je Osoba, což je možné u kompozice kde také platí vztah HAS-A. Ale u dědičnsoti musí jít vždy jen o vztah IS-A.

```cpp
#include <string>
#include <print> // c++23

using namespace std;

class Osoba
{
public:
    int cisloUctu;
    string jmeno;

    Osoba(int cisloUctu, string jmeno): cisloUctu(cisloUctu), jmeno(jmeno)
    {
    }
};

class Student
{
public:
    Osoba osoba;
    string skupina;

    Student(int cisloUctu, string jmeno, string skupina) : osoba(cisloUctu, skupina), skupina(skupina)
    {
    }

    void vypis()
    {
        println("osoba: {} {} skupina: {}", osoba.cisloUctu, osoba.jmeno, skupina);
    }
};

int main()
{
    Student student(123, "Alena", "AXP1");
    student.osoba.jmeno = "Tereza";
    student.vypis();
}
```



