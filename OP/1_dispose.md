
# Dispose pattern

Životní cyklus objektu (lifetime) představuje čas mezi vytvoření a zničením objektu. 

## Dispose Pattern

Dispose Pattern slouží k bezpečnému uvolnění zdrojů (paměti, připojení k serveru, k databázi atd.) i v případě výjimky.
V jazyce C++ ke stejnému účelu používáme destruktor, který se zavolá automaticky předtím, než se uvolní objekt z paměti.
V jazyce C# sice máme destruktor také, ale nevíme kdy přesně se zavolá, protože nevíme kdy Garbage Collector uvolní paměť objektu. 

Uvolnění zdrojů se provádí voláním metody `Dispose` z rozhraní `IDisposable`. A ke správnému volání této metody slouží příkaz `using`.

Nejprve si ukážeme příklad použití bez příkazu `using`. 

```cs 
static void Main(string[] args)
{
    int x = 1;

    if (x > 0)
    {
        Console.WriteLine(x);
    }
}
```

---
Více o problematice programovacích jazyků se můžete dozvědět napřílad v této knize:

[Michael L. Scott. 2009. Programming Language Pragmatics](https://www.cs.rochester.edu/~scott/pragmatics/)

Více se o Garbage Collectoru v jazyce C# dozvíte zde:

[Fundamentals of garbage collection, 2022](https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals)


