# Komunikace mezi nezávislými komponentami

Někdy potřebujeme aby spolu vzájemně komunikovaly komponenty aniž by měli na sebe referenci. Příkladem může být například komponenta zobrazující produkty v eshopu pro objednání a komponenta reprezentující košík, která ukazuje počet objednávek v košíku.

## Observer

Komunikaci můžeme vyřešit pomocí třídy s eventem, kdy katalog vyvolává event a košík je k tomuto eventu zaregistrovaný. 

Zde je ukázka třídy s eventem:

```csharp
public class ProductService
{
    public event Action? ProductAdded;

    public void NotifyProductAdded()
    {
        ProductAdded?.Invoke();
    }
}
```

Třídu ```ProductService``` zaregistrujeme do IoC kontejneru s lifetimem *Scoped*:

```csharp
builder.Services.AddScoped<ProductService>();
```

V komponentě ```Catalog``` potom injektujeme ```ProductService``` a vyvoláváme event voláním metody ```ProductService.NotifyProductAdded()```:

```razor
@inject ProductService ProductService

<h3>Catalog</h3>

<button @onclick="AddToBasket">Order Product</button>

@code {
    public void AddToBasket()
    {
        ProductService.NotifyProductAdded();
    }
}
```

Ve komponentě ```Basket``` potom při inicializaci zaregistrujeme metodu ```ProductAdded``` k eventu pomocí příkazu ```ProductService.ProductAdded += ProductAdded``` a také implementujeme rozhraní ```IDisposable``` a v metodě ```Dispose``` metodu ```ProductAdded``` odregistrujeme ```ProductService.ProductAdded -= ProductAdded```. V metodě ```ProductAdded``` musíme zavolat metodu ```StatHasChanged``` aby se zaktualizovalo uživatelské rozhraní a protože metoda ```ProductAdded``` může být zavolána z jiného vlákna, tak je použita metoda ```InvokeAsync```.

```razor
<h3>Basket</h3>
@implements IDisposable
@inject ProductService ProductService

orders: @orders
<br/>

@code {
    public int orders;

    protected override void OnInitialized()
    {
        ProductService.ProductAdded += ProductAdded;
    }

    public void ProductAdded()
    {
        ++orders;
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ProductService.ProductAdded -= ProductAdded;
    }
}
```

## Callback

Pokud mají obě komponenty, jak ```Basket``` tak ```Catalog``` stejného předka a jsou na stejné úrovni v hirerachii komponent, tak můžeme použít i callback. Typ parametru pro callback je v Blazoru ```EventCallback``` jak můžeme vidět v kódu komponenty ```Catalog```:

```razor
<h3>Catalog</h3>

<button @onclick="AddToBasket">Order Product</button>

@code {
    [Parameter]
    public EventCallback OnOrdered { get; set; }

    public void AddToBasket()
    {
        OnOrdered.InvokeAsync();
    }
}
```

Komponenta ```Basket``` má potom parametr ```Orders``` představující počet objednávek:

```razor
<h3>Basket</h3>

@Orders
<br/>

@code {
    [Parameter]
    public int Orders { get; set; }
}
```

Nadřazená komponenta ```Home``` má field ```orders``` reprezentující počet objednávek. Metoda ```ProductAdded``` je nabindovaná na parametr ```OnOrdered``` a field ```orders``` je nabindovaný na parameter ```Orders```:

```razor
@page "/"
@rendermode InteractiveServer

<PageTitle>Home</PageTitle>

<Catalog OnOrdered="ProductAdded" />
<Basket Orders="orders"/>

@code {
    int orders = 0;

    public void ProductAdded()
    {
        ++orders;
    }
}
```

---
1. [Need your Blazor sibling components to talk to each other?](https://jonhilton.net/blazor-sibling-communication/)