// Ukol 1: Prepracujte tridu Sklad na generickou, tak aby byl validni klientsky kod v metode Main

class Sklad<T>
{
    T[] data;
    private int pocet;

    public Sklad(int kapacita)
    {
        data = new T[kapacita];
    }

    public void Zaloz(T objekt)
    {
        data[pocet++] = objekt;
    }

    public T Vyloz()
    {
        return data[--pocet];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Sklad<int> skladInt = new Sklad<int>(10);
        skladInt.Zaloz(1);
        int celeCislo = skladInt.Vyloz();

        Sklad<string> skladString = new Sklad<string>(10);
        skladString.Zaloz("Ahoj");
        string retezec = skladString.Vyloz();
    }
}
