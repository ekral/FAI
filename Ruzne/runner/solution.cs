using System;

class Solution
{
    public static void KlientskyKod()
    {
        Manazer m = new()
        {
            Jmeno = "Petr"
        };

        m.VypisUdaje();
        m.VypisHeslo();
    }
}

class Zamestnanec
{
    public string Jmeno {get; set;} = string.Empty;
    protected decimal plat = 50000m;
    private string heslo = "abc123";

    public void VypisHeslo()
    {
        TestConsole.WriteLine($"Heslo: {heslo}");
    }
}

class Manazer : Zamestnanec
{
    public void VypisUdaje()
    {
        TestConsole.WriteLine($"Jmeno: {Jmeno}, Plat: {plat}");
    }
}