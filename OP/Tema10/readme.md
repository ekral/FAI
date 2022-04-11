## Task

Pomocí třídy `Task` můžeme spouštět typicky asynchronně jednu operaci (metodu) bez návratové hodnoty. 

V následujícím příkladu spustíme provádění metody Metoda pomocí statické metody `Task.Run`, tato metoda nám vrátí proměnnou task, krerá popisuje již spuštěnou operaci běžící typicky v jiném vlákně. Kompilátor se totiž může rozhodnout, zda se vplatní operaci pouštět v jiném vlákně nebo stačí ji provést sychronně ve stejném vlákně.

```cs 
static void DlouhaMetoda()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);
        Thread.Sleep(1000);
    }
}

static void Main(string[] args)
{
    Task task = Task.Run(DlouhaMetoda);
    Console.WriteLine("Cekam na stisk klavesy");
    Console.ReadKey();
}
```

V předcházejícím kódu souběžně v jednom vklákně na konzoli zapisujeme a v jiném čekáme na vstup z klávesy. Je to možné proto, že konzole zápis a čtení synchronizuje [(Console I/O Streams. Microsoft Docs. 2022)](https://docs.microsoft.com/en-us/dotnet/api/system.console?view=net-6.0#console-io-streams). Pokud bychom ale chtěli počkat na dokončení provádění metody Metoda a teprve potom čekat na stisk klávesy, tak můžeme použít klíčové slovo `await`. Klíčové slovo await můžeme použít jen v metodě označené klíčovým slovem `async`. V následujícím příkladu si všimněte, že metoda  `Main` je označena `async` a návratový typ má `Task`, tento návratový typ si vysvětlíme později.

```cs 
static void DlouhaMetoda()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);
        Thread.Sleep(400);
    }
}

static async Task Main(string[] args)
{
    Task task = Task.Run(DlouhaMetoda);
    await task;
    Console.WriteLine("Cekam na stisk klavesy");
    Console.ReadKey();
}
```

Příkaz `await task;` nepozastaví běh programu, ale jen přeruší provádění metody `Main` a mezití běží ostatní operace a po dokončení metody `Metoda` bude v provádění metody `Main` pokračovat. 


## Task s návratovou hodnotou
    
Pokud metoda, která má proběhnout asynchroně vrací hodnotu, tak používáme třídu `Task<TResult>`. V následujícím příkladu vrací metoda `Vypocet` celočíselnou hodnotu. Metoda `Run` potom vrací typ `Task<int>` a když potom použiji klíčové slovo `await` tak, až se výpočet dokončí, tak získám přímo hodnotu typu `int`.

```cs 
static int DlouhyVypocet()
{
    Thread.Sleep(1000);
    return Random.Shared.Next();
}

static async Task Main(string[] args)
{
    Task<int> task = Task.Run(DlouhyVypocet);
    int x = await task;

    Console.WriteLine($"Vysledek je {x}");
}
```

Zápis může být zkrácen na jeden řádek:
```cs 
int x = await Task.Run(Vypocet);
```

## Návratová hodnota asynchronních metod

V následujícím příkladu jsme si vytvořili asynchronní metodu `VypocetAsync`, kde vracíme přímo typ `Task<int>`. 

```cs 
static int DlouhyVypocet()
{
    Thread.Sleep(1000);
    return Random.Shared.Next(0, 100);
}

static Task<int> VypocetAsync()
{
    return Task.Run(DlouhyVypocet);
}

static async Task Main(string[] args)
{
    int x = await VypocetAsync();

    Console.WriteLine($"Vysledek je {x}");
}
```

Jak by ale vypadal návratový typ, když bychom nevraceli přímo `Task<int>`, ale chtěli provést dva výpočty v daném pořadí? V následujícím příkladu musíme použít klíčové slovo `await`, abychom pak mohli ještě výsledek umocnit. Všimněte si, že i když vracíme přímo `vysledek`, tedy typ `int`, tak protože jsme použili klíčové slovo `await`, tak návratový typ asynchornní metody je `Task<int>`.

```cs 
static int DlouhyVypocet()
{
    Thread.Sleep(1000);
    return Random.Shared.Next(0, 100);
}

static async Task<int> VypocetAsync()
{
    int vysledek = await Task.Run(DlouhyVypocet);
    return vysledek * vysledek;
}

static async Task Main(string[] args)
{
    int x = await VypocetAsync();

    Console.WriteLine($"Vysledek je {x}");
}
```

Pokud metoda nevrací žádnou hodnotu, tak mám dvě možnosti, buď použít návratový typ void nebo `Task`. Doporučuje se používat návratový typ `Task`, protože jinak bychom nemohli použít s metodou `await` a provést nějakou operaci po dokončení této metody.

V následujícím příkladu máme metodu  `MetodaAync `, ve které chceme po provedení asychroní operace počkat na stisk klávesy. Metoda nevrací žádnou hodnotu a v metodě Main na ni nemůžeme počkat a program se hned ukončí. Také nemužeme ošetřit výjimky vyvolané v asynchornní metodě s návratovým typem void.

```cs 
static void DlouhaMetoda()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);
        Thread.Sleep(400);
    }
}

static async void MetodaAsync()
{
    await Task.Run(DlouhaMetoda);
    Console.WriteLine("Konec vypoctu");
}

static async Task Main(string[] args)
{
    MetodaAsync();
    // program se uKonci, nemame zadny task s kterym pracovat
}
```

Pokud změníme návratový typ asynchronní metody `MetodaAsync` na `Task`, tak s ním můžeme v metodě `Main` použít `await` a program se neukončí.

```cs 
static void DlouhaMetoda()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);
        Thread.Sleep(400);
    }
}

static async Task MetodaAsync()
{
    await Task.Run(DlouhaMetoda);
    Console.WriteLine("Konec vypoctu");
}

static async Task Main(string[] args)
{
    await MetodaAsync();
}
```

## Příklad

V následujícím příkladu získáváme z webového API pomocí asynchronní metody `GetStringAsync` text ve formátu json, který potom vypíšeme na konzoli. Pro ukázku je uvedené i ošetření výjimky. V případě použití klíčového slova `using` musíme použít klíčové slovo `await` a nevracet přímo `Task<string>` tak by mohla být zavolána metoda `Dispose`.

```cs 
static async Task<string> NactiVtipAsync()
{
    using (HttpClient client = new HttpClient())
    {
        try
        {
            string vtip = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=json");
            return vtip;
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            throw;
        }
    }
}

static async Task Main(string[] args)
{
    string vtip = await NactiVtipAsync();
    Console.WriteLine(vtip);
}
```

Více se o typu Task a klíčových slovech můžete dozvědět například zde:

[Task-based asynchronous pattern. Microsoft Docs. 2022](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming)

A zde najdete přehled best practice asynchronního programování v jazyce C#:

[Async/Await - Best Practices in Asynchronous Programming. MSDN Magazine Issues. 2013](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
