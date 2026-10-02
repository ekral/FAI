int main()
{
    // Studenti psali 10 testu, kazdy test byl za 0 az 10 bodu.
    // Student musel ziskat z prumeru 10 testu 5 bodu (0 az 10 bodu)
    // Dochazku musel mit na 80% procent.

    // Vysledna znamka byla
    // <9,10>   A vyborne
    // <8,9)    B velmi dobre
    // <7,8)    C dobre
    // <6,7)    D uspokojive
    // <5,6)    E dostatecne
    // <0,5)    F nedostatecne

    double prumerTestu = 7.0;
    double dochazka = 90.0;

    // Vypiste slovni hodnoceni
    if (dochazka < 80.0)
    {
        puts("Nesplnil dochazku");
    }
    else
    {
        if (prumerTestu >= 9)
        {
            puts("A vyborne");
        }
        else if (prumerTestu >= 8)
        {
            puts("B velmi dobre");
        }
        else if (prumerTestu >= 7)
        {
            puts("C dobre");
        }
        else if (prumerTestu >= 6)
        {
            puts("D uspokojive");
        }
        else if (prumerTestu >= 5)
        {
            puts("E dostatecne");
        }
        else
        {
            puts("F nedostatecne");
        }
    }   
}
