// Ukol 1: Prepracujte tridu Sklad na generickou, tak aby byl validni klientsky kod v metode Main

class Sklad
{
    int[] data;
    private int pocet;

    public Sklad(int kapacita)
    {
        data = new int[kapacita];
    }

    public void Zaloz(int objekt)
    {
        data[pocet++] = objekt;
    }

    public int Vyloz()
    {
        return data[--pocet];
    }
}

class Program
{
        static void Main(string[] args)
        {
            Sklad<int>  skladInt = new Sklad<int>(10);
            skladInt.Zaloz(1);
            int celeCislo = skladInt.Vyloz();

            Sklad<string> skladString = new Sklad<string>(10);
            skladString.Zaloz("Ahoj");
            string retezec = skladString.Vyloz();
        }
}
