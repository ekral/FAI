#include <stdio.h>

struct BankovniUcet
{
    int cislo;
    double zustatek; // nemenit primo, pouzivat metody vloz a vyber
};

void init(struct BankovniUcet* this, int cislo)
{
    this->cislo = cislo;
    this->zustatek = 0.0;
}

void vloz(struct BankovniUcet* this, double castka)
{
    this->zustatek += castka;
}

void vyber(struct BankovniUcet* this, double castka)
{
    if (castka <= this->zustatek)
    {
        this->zustatek -= castka;
    }
}

int main()
{
    // instance na zasobniku
    struct BankovniUcet ucet;
    init(&ucet, 1);
    vloz(&ucet, 2000);
    vyber(&ucet, 200);
    
    // ucet.zustatek -= 200; // jde prelozit

    printf("zustatek: %lf\n", ucet.zustatek);
}
