MojeTrida trida = new MojeTrida();

trida.UdalostAction += (x, y) => Console.WriteLine($"x: {x} y: {y}");
trida.UdalostFunction += (x, y) => x + y;
trida.UdalostPredicate += x => x > 0; 
trida.UdalostFunc2 += () => 1;
trida.UdalostFunc2 += () => 2;
trida.UdalostFunc2 += () => 3;

trida.Metoda();
Console.ReadKey(true);

class MojeTrida
{
    public event Action<double, double>? UdalostAction;
    public event Func<double, double, double>? UdalostFunction;
    public event Predicate<int>? UdalostPredicate;
    public event Func<int>? UdalostFunc2;

    public void Metoda()
    {
        UdalostAction?.Invoke(10, 5);
        double vysledek1 = UdalostFunction?.Invoke(10.0, 5.0) ?? 0.0;
        Console.WriteLine(vysledek1);

        bool vysledek2 = UdalostPredicate?.Invoke(2) ?? false;
        Console.WriteLine(vysledek2);

        int vysledek3 = UdalostFunc2?.Invoke() ?? 0;
        Console.WriteLine(vysledek3);

    }
}
