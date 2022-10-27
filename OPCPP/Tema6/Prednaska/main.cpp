#include <stdio.h>
#include <vector>

class Ctverec
{
public:
    int a;

    Ctverec() : a(0)
    {

    }

    Ctverec(int a) : a(a)
    {

    }
};

void Vypis(Ctverec* c)
{
    printf("ctverec.a: %d\n", c->a); // a je delka ctverec
}

void Zmen(Ctverec c)
{
    c.a = 0;
}

void Zmen(int* p)
{
    *p = 0; // operator indirekce
}

struct Matice
{
    int pocetRadku = 10;
    int pocetSloupcu;
    int* pole;

    Matice(int pocetRadku, int pocetSloupcu) 
        : pocetRadku(pocetRadku),pocetSloupcu(pocetSloupcu)
    {
        pole = new int[pocetRadku * pocetSloupcu];
    }

    ~Matice()
    {
        delete[] pole;
    }
};

int main()
{
    Ctverec c2(3);
    Ctverec* pc = new Ctverec(3);

    delete pc;
    
    Ctverec* ctverce = new Ctverec[100];
    delete[] ctverce;

    std::vector<Ctverec> vc;
    vc.push_back(Ctverec(2));
    
    Matice matice(2, 3);

    int x = 1;
    Zmen(&x);
    printf("x: %d\n", x);
    Ctverec c(2);
    c.a = 1;
    Zmen(c);
    Vypis(&c); // adresni operator (operator reference)
}
