Func<int, int, int> d = Secti;
int vysledek1 = d.Invoke(2, 3);

Action<string> d2 = Vypis;
d2.Invoke("ahoj");

Action d3 = VypisStejny;
d3.Invoke();

Func<int, bool> d4 = JeKladne;
bool vysledek2 = d4.Invoke(6);

Predicate<int> d5 = JeKladne;

List<int> cisla = new List<int> { -5, 2, 3, 4, -8, 0 };
List<int> kladna = cisla.Where(x => x > 0).ToList();

int min = 2; // capture local variable
List<int> vetsi = cisla.Where(x => x > min).ToList();

Func<int, bool> d6 = VratFunkci();
List<int> vetsi2 = cisla.Where(d6).ToList();

Console.WriteLine(String.Join(",", kladna));

Hlidac hlidac = new Hlidac();

hlidac.poplach += ZpracujPoplachPolicie;
hlidac.poplach += ZpracujPoplachHasici;
//hlidac.poplach -= ZpracujPoplachHasici;
//hlidac.poplach.Invoke();
//hlidac.poplach = null;

hlidac.Vloupani();


void ZpracujPoplachPolicie()
{
    Console.WriteLine("Jede policie");
}

void ZpracujPoplachHasici()
{
    Console.WriteLine("Jedou hasici");
}

Func<int, bool> VratFunkci()
{
    int min = 2; // capture, captured lokalni promenne maji delsi lifetime
    Func<int, bool> vetsi = x => x > min;
    return vetsi;
}

int Secti(int x, int y)
{
    return x + y;
}

void Vypis(string text)
{
    Console.WriteLine(text);
}

void VypisStejny()
{
    Console.WriteLine("Ahoj");
}

bool JeKladne(int x)
{
    return x > 0;
}
//delegate int MujDelegate(int x, int y);

class Hlidac
{
    public event Action poplach;

    public void Vloupani()
    {
        poplach?.Invoke();
    }
}
