#include <stdio.h>

#define MAX(a,b) (((a) > (b)) ? (a) : (b))

// v c++ nemusim pouzivat typedef

struct Student
{
    const char* jmeno;
    double test1;
    double test2;
};

void vypis(Student* studenti, int pocet)
{
    for (int i = 0; i < pocet; i++)
    {
        printf("%-10s test1: %.1lf test2: %.1lf\n", studenti[i].jmeno, studenti[i].test1, studenti[i].test2);
    }
}

int main(void)
{
    double vetsi = MAX(10, 5);

    Student studenti[] =
    {
        {"Bronislav", 70.0, 50.0},
        {"Ondrej", 30.0, 50.0},
        {"Zdenek", 70.0, 90.0},
        {"Michaela", 50.0, 50.0}
    };

    vypis(studenti, 4);

    // Najdete a vypiste jmeno studenta s nejlepsim vysledkem ze dvou testu
    int index_nejlepsiho = 0;
    double nejlepsi_test = MAX(studenti[0].test1, studenti[0].test2);

    for (int i = 1; i < 4; i++)
    {
        double vetsi = MAX(studenti[i].test1, studenti[i].test2);

        if (vetsi>nejlepsi_test) 
        {
            index_nejlepsiho = i;
            nejlepsi_test = vetsi;
        }
    }

    printf("Nejlepsi je %s s %.2lf body\n", studenti[index_nejlepsiho].jmeno,nejlepsi_test);

    return 0;
}
