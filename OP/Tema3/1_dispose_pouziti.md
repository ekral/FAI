# Použití metody Dispose

Dispose Pattern slouží k bezpečnému uvolnění zdrojů (paměti, připojení k serveru, k databázi atd.) i v případě výjimky.
V jazyce C++ ke stejnému účelu používáme destruktor, který se zavolá automaticky předtím, než se uvolní objekt z paměti.
V jazyce C# sice máme destruktor také, ale nevíme kdy přesně se zavolá, protože nevíme kdy Garbage Collector uvolní paměť objektu. 

Uvolnění zdrojů se provádí voláním metody `Dispose` z rozhraní `IDisposable`. A ke správnému volání této metody slouží příkaz `using`.

Nejprve si ukážeme příklad volání metody Dispose bez příkazu `using`. V následujícím příkazu pomocí třídy `HttpClient` zavoláme webovou službu a zobrazíme získaný řetězec. Blok finally se zavolá vždy, i když dojde nebo nedojde k výjimce při volání metody `GetStringAsync`. 

```cs 
static async Task Main(string[] args)
{
    string url = "https://geek-jokes.sameerkumar.website/api?format=json";

    HttpClient client = new HttpClient();

    try
    {
        string jsonString = await client.GetStringAsync(url);
        Console.WriteLine(jsonString);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        client.Dispose();
    }
}
```

A nyní použijeme klíčové slovo `using`, jehož syntaxe je jednoduší a zajistí, že i v případě výjimky bude zavolaná metoda Dispose a bude řádně ukončeno připojení k serveru. 

```cs 
static async Task Main(string[] args)
{
    string url = "https://geek-jokes.sameerkumar.website/api?format=json";

    using (HttpClient client = new HttpClient())
    {
        try
        {
            string jsonString = await client.GetStringAsync(url);
            Console.WriteLine(jsonString);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
```
---
Více o metodě dispose najdete například zde:

[IDisposable.Dispose Method, 2022](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable.dispose?view=net-6.0)


