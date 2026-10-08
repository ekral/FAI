using System;

class Solution
{
    public static void KlientskyKod()
    {
        NakladniVozidlo vozidlo = new("Tatra", "Phoenix", 11000);
    }
}

class Vozidlo
{
    public string Znacka {get; } = string.Empty;
    public string Model {get; } = string.Empty;
    
    public Vozidlo(string znacka, string model)
    {
        Znacka = znacka;
        Model = model;
    }
}

class NakladniVozidlo : Vozidlo
{
    public int Nosnost {get; }

    public NakladniVozidlo(string znacka, string model, int nosnost) : base(znacka, model)
    {
        Nosnost = nosnost;
    }
}

