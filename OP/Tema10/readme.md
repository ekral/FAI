## Task

Pomocí třídy `Task` můžeme spouštět typicky asynchronně spouštět jednu operaci (metodu) bez návratové hodnoty. 

V následujícím příkladu spustíme provádění metody Metoda pomocí statické metody `Task.Run`, tato metoda nám vrátí proměnnou task, krerá popisuje již spuštěnou operaci běžící typicky v jiném vlákně. Kompilátor se totiž může rozhodnout, zda se vplatní operaci pouštět v jiném vlákně nebo stačí ji provést sychronně ve stejném vlákně.

```cs 
static void Metoda()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);
        Thread.Sleep(1000);
    }
}

static void Main(string[] args)
{
    Task task = Task.Run(Metoda);
    Console.WriteLine("Cekam na stisk klavesy");
    Console.ReadKey();
}
```

V předcházejícím kódu souběžně v jednom vklákně na konzoli zapisujeme a v jiném čekáme na vstup z klávesy. Je to možné proto, že konzole zápis a čtení synchronizuje [1](https://docs.microsoft.com/en-us/dotnet/api/system.console?view=net-6.0#console-io-streams). Pokud bychom ale chtěli počkat na dokončení provádění metody Metoda a teprve potom čekat na stisk klávesy, tak můžeme použít klíčové slovo `await`.

```cs 
static void Metoda()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);
        Thread.Sleep(400);
    }
}

static async Task Main(string[] args)
{
    Task task = Task.Run(Metoda);
    await task;
    Console.WriteLine("Cekam na stisk klavesy");
    Console.ReadKey();
}
```

