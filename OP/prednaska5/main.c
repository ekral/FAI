#include <stdio.h>

struct zviratko
{
    char* jmeno;
    char* (*zvuk)(struct zviratko*);
};

struct pejsek
{
    struct zviratko zviratko;
    int bouda;
};

char* pejsek_zvuk(struct pejsek* pejsek)
{
    return "haf";
}

struct kocicka
{
    struct zviratko zviratko;
    int zachod;
};

char* kocicka_zvuk(struct kocicka* kocicka)
{
    return "mnau";
}

void vypis(struct zviratko* zviratko)
{
    char* zvuk = zviratko->zvuk(zviratko);
    printf("%s %s\n", zviratko->jmeno, zvuk);
}

int main()
{
    struct pejsek pejsek = { .zviratko.jmeno = "alik", .zviratko.zvuk = pejsek_zvuk, .bouda = 1 };
    struct kocicka kocicka = { .zviratko.jmeno = "micka", .zviratko.zvuk = kocicka_zvuk, .zachod = 1 };

    struct zviratko* zviratka[] = { &pejsek, &kocicka };
  
    for (struct zviratko** p = &zviratka[0]; p < &zviratka[0] + 2; p++)
    {
        vypis(*p);
    }
}
