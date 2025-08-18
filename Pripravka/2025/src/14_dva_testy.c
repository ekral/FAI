int main()
{
    // student psal testy, mohl ziskat 0 az 100%
    double test1 = 40;
    double test2 = 60;

    // Aby student splnil podminky, tak kazdy z testu
    // musel byt splneny na vice nez 50%

    // napiste kod, ktery vypise text 
    // "splnil" nebo "nesplnil"
    // && je operator "a zaroven"
    if (test1 > 50 && test2 > 50)
    {
        puts("splnil");
    }
    else
    {
        puts("nesplnil");
    }

    // studentovi staci splnit jen jeden ze dvou testu
    // || je operator "a nebo"
    if (test1 > 50 || test2 > 50)
    {
        puts("splnil");
    }
    else
    {
        puts("nesplnil");
    }
}
