using System;

class Solution
{
    public static void KlientskyKod()
    {
        int result = 0;

        //Vypocet v0 = new Vypocet(); // nepujde prelozit
        
        Vypocet v1 = new Soucet();  
        result = v1.Vypocitat(3, 4); // result bude 7

        Vypocet v2 = new Soucin();
        
        result = v2.Vypocitat(3, 4); // result bude 12
        Soucin v3 = new Soucin();

        result = v3.Vypocitat(3, 4); // result bude 12
        
        Soucet v4 = new Soucet();
        result = v4.Vypocitat(3, 4); // result bude 7
    }
}

abstract class Vypocet
{
    public abstract int Vypocitat(int x, int y);
}

class Soucet() : Vypocet
{
    public override int Vypocitat(int x, int y)
    {
        return x + y;
    }
}

class Soucin : Vypocet
{
    public override int Vypocitat(int x, int y)
    {
        return x * y;
    }
}