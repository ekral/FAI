

class Solution
{
    public static void KlientskyKod()
    {
        string zvuk = string.Empty;

        Zviratko z1 = new Zviratko("Fik");
        zvuk = z1.Zvuk(); // Metoda by měla vrátit jméno zvířátka, tedy "Fik"

        Zviratko z2 = new Pejsek("Rex");
        zvuk = z2.Zvuk(); // Metoda by měla vrátit jméno zvířátka, tedy "Rex"
        
        Pejsek z3 = new Pejsek("Max");
        zvuk = z3.Zvuk(); // Metoda by měla vrátit "Max dela haf haf!"  
    }
}

class Zviratko(string jmeno)
{
    public string Jmeno { get; set; } = jmeno;
    public string Zvuk()
    {
        return Jmeno;
    }
}

class Pejsek(string jmeno) : Zviratko(jmeno)
{
    public string Zvuk()
    {
        return $"{Jmeno} dela haf haf!";        
    }
}