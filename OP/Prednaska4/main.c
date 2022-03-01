struct osoba
{
    char* jmeno;
    char* prijmeni;
};

struct student
{
    struct osoba osoba;
    char* skupina;
};

int main()
{
    struct student student1;
    student1.osoba.jmeno = "Peppa";
    student1.osoba.prijmeni = "Prasatko";
    student1.skupina = "A1XPA";

    // inicializace v C
    struct student student1 = { .osoba.jmeno = "Jan", .osoba.prijmeni = "Novy", .skupina = "A1XPA" };
}
