#include <stdio.h>

template<typename T>
struct Cislo
{
    T x;
};


struct Zviratko
{
    int vek;

    virtual void Zvuk()
    {
        printf("abstraktni zviratko nedela zadny zvuk\n");
    }
};

struct Pejsek : public Zviratko
{
    const char* jmeno;

    void Zvuk() override
    {
        printf("%s dela haf haf\n", jmeno);
    }
};


struct Kocicka : public Zviratko
{
    const char* jmeno;

    void Zvuk()
    {
        printf("%s dela mnau\n", jmeno);
    }
};

int main()
{
    Cislo<int> cislo1;
    Cislo<double> cislo2;

    Pejsek p1;
    p1.vek = 6;
    p1.jmeno = "Punta";
    //p1.Zvuk();

    Kocicka k1;
    k1.vek = 7;
    k1.jmeno = "Micka";

    Zviratko* z = &k1;
    //z->Zvuk();

    Zviratko* zviratka[] = { &p1, &k1 };

    for (size_t i = 0; i < 2; i++)
    {
        zviratka[i]->Zvuk();
    }
}
