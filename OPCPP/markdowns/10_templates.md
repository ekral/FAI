# 10 Šablony (templates)

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

```c++
#include <vector>
#include <iostream>

using namespace std;

int main()
{
    vector<int> cisla; // na zacatku nema vector zadne prvky
    cisla.push_back(1); // pridam 1 na konec, rychla operace
    cisla.push_back(2);
    cisla.push_back(3);
    cisla.push_back(4);
    cisla.push_back(5);

    cisla.pop_back(); // odeberu posledni

    cisla.insert(cisla.begin() + 1, 7); // vlozim 7 mezi prvni a druhe cislo, pomalejsi operace
    cisla.erase(cisla.begin() + 2); // odstranim prvek s indexem 2 (treti prvek)
    cisla[0] = 9; // zmenim cislo s indexem 0

    cout << cisla[2] << endl; // vypise cislo s indexem 2

    for(const int cislo : cisla)
    {
        cout << cislo << endl;
    }
}
```

## Datový kontejner map

Datový kontejner map představuje asociativní pole klíč a hodnota. Používáme jej, když chceme rychle hledat hodnoty dle klíče.

```cpp
#include <map>
#include <string>
#include <iostream>

using namespace std;

int main()
{
    map<int, string> slovnik;

    slovnik[10] = "Karel";
    slovnik[12] = "Jiri";
    slovnik[20] = "Alena";

    cout << slovnik[12] << endl;

    // c++17
    for (const auto& [key, value] : slovnik)
    {
        cout << key << ": " << value << endl;

    }

    // C++11
    for (const pair<int,string> zaznam : slovnik)
    {
        cout << zaznam.first << ": " << zaznam.second << endl;
    }

    cout << slovnik.contains(20) << endl;

    slovnik.erase(20);

    cout << slovnik.contains(20) << endl;

    slovnik.clear();

    return 0;
}
```