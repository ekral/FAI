using System;

class Solution
{
    public static void KlientskyKod()
    {
        Faktura f1 = new(3, 1500.50m, "Petr", "petr@example.com");

        TestConsole.WriteLine($"Pocet polozek: {f1.PocetPolozek} Cena celkem: {f1.CenaCelkem} {f1.Zakaznik?.Jmeno}, Email: {f1.Zakaznik?.Email}");
    }
}

class Zakaznik(string jmeno, string email)
{
    public string Jmeno {get; } = jmeno;
    public string Email {get; } = email;
}

class Faktura(int pocetPolozek, decimal cenaCelkem, string jmeno, string email)
{
    public int PocetPolozek {get; } = pocetPolozek;
    public decimal CenaCelkem {get; } = cenaCelkem;
    public Zakaznik Zakaznik {get; } = new(jmeno, email);
}

