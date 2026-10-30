

class Solution
{
    public static void KlientskyKod()
    {
        string vysledek = string.Empty;

        Rodic t1 = new Rodic();
        vysledek = t1.Metoda(); // Metoda by měla vrátit "Rodic"

        Rodic t2 = new Potomek();
        vysledek = t2.Metoda(); // Metoda by měla vrátit "Rodic"
        
        Potomek t3 = new Potomek();
        vysledek = t3.Metoda(); // Metoda by měla vrátit "Potomek"  
    }
}

class Rodic
{
    public string Metoda()
    {
        return "Rodic";
    }
}

class Potomek : Rodic
{
    public new string Metoda()
    {
        return "Potomek";     
    }
}