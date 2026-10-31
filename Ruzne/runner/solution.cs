using System;

static class Solution
{
    public static void Test(int a, int b)
    {
        Zviratko z1 = new Zviratko("Fik");
        z1.Zvuk();
        Zviratko z2 = new Pejsek("Rex");
        z2.Zvuk();
        Pejsek z3 = new Pejsek("Max");
        z3.Zvuk();
    }
}

class Zviratko(string jmeno)
{
    public string Jmeno { get; set; } = jmeno;
    public virtual void Zvuk()
    {
        TestConsole.WriteLine(Jmeno);
    }
}

class Pejsek(string jmeno) : Zviratko(jmeno)
{
    public override void Zvuk()
    {
        TestConsole.WriteLine($"{Jmeno} dela haf haf!");
    }
}