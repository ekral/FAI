# Dedičnost kódu

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

- Modifikátor přístupu protected se používá pouze v souvislosti s dědičností.
- K protected členským prvkům může vývojář přistupovat v třídě potomka, ale stejně jako u private k nim nemůže přistupovat mimo třídu rodiče nebo potomka. 

