# Persist Prerendered State

Blazor používá z důvodu větší rychlosti odezvy a podpoře Search Engine Optimization (SEO) prerenderování. Znamená to, že nejprve vyrenderuje html stránku na straně serveru a odešle ji na klienta a teprve poté zprovozní SignalR kanál u ```InteractiveServer``` render módu a nebo nahraje do prohlížeče WebAssemly v případě ```InteractiveWebAssembly``` render módu.

Prakticky to znamená, že se stránka vyrenderuje dvakrát, nejprve na straně serveru a poté na straně klienta. Metoda ```OnIntialized``` se provede také dvakrát. Následující příklad to demonstruje. V metodě ```OnInitialized``` generujeme náhodné číslo pro čítač v ```InteractiveServer``` render módu. Protože se metoda ```OnInitialized``` zavolá dvakrát, tak pokud aplikaci spustíme, tak uvidíme, že na chvíli se zobrazí jedno náhodné číslo a potom druhé náhodné číslo.

```razor
@page "/"
@rendermode InteractiveServer

<PageTitle>Counter</PageTitle>

<h1>Counter</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    protected override void OnInitialized()
    {
        currentCount = Random.Shared.Next(100);
    }
    private void IncrementCount()
    {
        currentCount++;
    }
}
```
## PersistentComponentState

V Blazoru máme možnost aplikační stav (například hodnotu proměnné ```currentCount```) serializovat na straně serveru a potom deserializovat na straně klienta. Výhodné je to také, pokud je inicializace náročná na zdroje, například provádí náročný dotaz do databáze.

V následujícím příkladu injektujeme službu ```PersistentComponentState``` s pomocí které potom serializujeme a přenášíme na klienta ze serveru hodnotu proměnné ```currentCount``` a poté na straně klienta tuto hodnotu získáme, deserializujeme a použijeme. Zápis ```PersistentComponentState.RegisterOnPersisting(PersistData)``` registruje metodu, která se má pro serializaci a přenos zavolat s tím, že referenci na tuto registraci si uložíme do fieldu ```PersistentComponentState``` tak, abychom pak mohli zavolat metodu ```Dispose``` a nedoslo k memory leaku.

```razor
@page "/"

@implements IDisposable
@rendermode InteractiveServer

@inject PersistentComponentState PersistentComponentState

<PageTitle>Counter</PageTitle>

<h1>Counter</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    PersistingComponentStateSubscription persistingComponentStateSubscription;


    protected override void OnInitialized()
    {
        persistingComponentStateSubscription = PersistentComponentState.RegisterOnPersisting(PersistData);

        if (!PersistentComponentState.TryTakeFromJson(nameof(currentCount), out int restored))
        {
            currentCount = Random.Shared.Next(100);
        }
        else
        {
            currentCount = restored;
        }
    }

    private void IncrementCount()
    {
        currentCount++;
    }

    private Task PersistData()
    {
        PersistentComponentState.PersistAsJson(nameof(currentCount), currentCount);

        return Task.CompletedTask;
    }

    void IDisposable.Dispose()
    {
        persistingComponentStateSubscription.Dispose();
    }
}
```