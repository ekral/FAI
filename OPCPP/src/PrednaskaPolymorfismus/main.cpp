#include <string>
#include <iostream>
#include <vector>
#include <algorithm>
#include <ranges>

using namespace std;

// Polymorfismus, mnohotvarost

class Zviratko
{
public:
    string jmeno;

    Zviratko(string jmeno) : jmeno(jmeno)
    {

    }

    // virtualni funkce jsou pomalejsi
    virtual void Zvuk()
    {

    }
};

class Pejsek : public Zviratko
{
public:
    bool bouda;

    Pejsek(string jmeno, bool bouda) : Zviratko(jmeno), bouda(bouda)
    {

    }

    void Zvuk() override
    {
        cout << "haf haf" << endl;
    }
};

class Kocicka : public Zviratko
{
public:
    int pocetZivotu;

    Kocicka(string jmeno) : Zviratko(jmeno), pocetZivotu(9)
    {

    }

    void Zvuk() override
    {
        cout << "mnau" << endl;
    }
};

class Kravicka : public Zviratko
{
public:
    Kravicka(string jmeno) : Zviratko(jmeno)
    {

    }

    void Zvuk()
    {
        cout << "buuu" << endl;
    }
};

int main()
{
    Pejsek pejsek("Alik", false);
    Kocicka kocicka("Garfield");
    Kravicka kravicka("Alenka");

    //pejsek.Zvuk();
    //kocicka.Zvuk();
    //kravicka.Zvuk();

    Zviratko* p = &pejsek; // upcasting
    p->Zvuk();

    vector<Zviratko*> zviratka{ &pejsek, &kocicka, &kravicka };

    for (Zviratko* zviratko : zviratka)
    {
        zviratko->Zvuk();
    }

    // Bonus nad ramec predmetu
    // Vyzaduje c++20 a vyssi
    sort(zviratka.begin(), zviratka.end(), [](Zviratko* z1, Zviratko* z2) { return z1->jmeno < z2->jmeno; });
    for_each(zviratka.begin(), zviratka.end(), [](Zviratko* z) { cout << z->jmeno << endl;  });

    cout << "Zviratka zacinajici na A" << endl;

    auto view = zviratka | views::filter([](Zviratko* z) { return z->jmeno.length() > 0 && z->jmeno[0] == 'A'; });

    for (Zviratko* zviratko : view)
    {
        cout << zviratko->jmeno << endl;
    }
}