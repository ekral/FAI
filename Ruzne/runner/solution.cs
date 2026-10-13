class Submission
{
    public static void Run(int cislo)
    {
        int pocetNasobeni = 0;
        int mocnina = 1;

        while (mocnina <= cislo)
        {
            pocetNasobeni++;
            mocnina *= 2;
        }

        TestConsole.WriteLine($"m: {mocnina}, pn: {pocetNasobeni}");
    }
}