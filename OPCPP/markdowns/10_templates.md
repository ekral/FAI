# Šablony (templates)

**autor Erik Král ekral@utb.cz**

---

Šablony v C++ nebo Generika (C#, Java) umožňují odložit přesnou definici použitého datového typu v rámci datového typu, například třídy nebo rozhraní. V jazyce C se pro podobné účely používá příkaz textového preprocecoru #define.

Šablony poskytují vetší znovu použitelnost kódu.. Nejčastější aplikace je v rámci kolekcí.

V následujícím příkladu je ukázka definice třídy sklad, který představuje zásobník s pevnou délkou.

```c++
class Sklad
{
private:
    int* data;
    int pocet;
public:
    explicit Sklad(const int kapacita) : pocet(0)
    {
        data = new int[kapacita];
    }

    void Zaloz(const int objekt)
    {
        data[pocet++] = objekt;
    }

    int Vyloz()
    {
        return data[--pocet];
    }
};

int main()
{
    Sklad sklad(10);
    sklad.Zaloz(1);
    sklad.Zaloz(2);
    sklad.Zaloz(3);
    const int cislo = sklad.Vyloz();
    printf("cislo: %d\n", cislo);
}
```

Pokud bychom chteli pouzit třídu `Sklad` i pro jiné typy, například `double` nebo `string` tak bychom museli vytvářet nové třídy. Díky šablonám ale můžeme vytvořit jen jednu třídu s generickým parametrem.

```c++
#include <compare>
#include <cstdio>
#include <iostream>
#include <string>

using namespace std;

template <typename  T>
class Sklad
{
private:
    T* data;
    int pocet;
public:
    explicit Sklad(const int kapacita) : pocet(0)
    {
        data = new T[kapacita];
    }

    void Zaloz(const T objekt)
    {
        data[pocet++] = objekt;
    }

    T Vyloz()
    {
        return data[--pocet];
    }
};

int main()
{
    Sklad<int> skladCisel(10);
    skladCisel.Zaloz(1);
    skladCisel.Zaloz(2);
    skladCisel.Zaloz(3);

    const int cislo = skladCisel.Vyloz();

    printf("cislo: %d\n", cislo);

    Sklad<string> skladRetezcu(10);
    skladRetezcu.Zaloz("ahoj");
    skladRetezcu.Zaloz("jak");
    skladRetezcu.Zaloz("je");

    string retezec = skladRetezcu.Vyloz();

    cout << retezec << endl;

}
```

## Datový kontejner vector

Jedním z příkladů použití šablon v C++ jsou datové konteinery ze standardní knihovny. Jde o dynamické pole, do kterého můžeme přidávat prvky a z kterého můžeme prvky odebírat.

