class Solution
{
    public static void KlientskyKod()
    {
        PotomekA a = new PotomekA()
        {
            X = 1,
            Y = 2
        };

        PotomekB b = new PotomekB()
        {
            X = 3,
            Z = 4
        };
    }
}

class Rodic
{
    public int X {get; set;}
}

class PotomekA : Rodic
{
    public int Y {get; set;}
}

class PotomekB : Rodic
{
    public int Z {get; set;}
}