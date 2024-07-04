# Komunikace mezi nezávislými komponentami

Někdy potřebujeme aby spolu vzájemně komunikovaly komponenty aniž by měli na sebe referenci. Příkladem může být například komponenta zobrazující produkty v eshopu pro objednání a komponenta reprezentující košík, která ukazuje počet objednávek v košíku.

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

Kterou zaregistrujeme s lifetimem Scoped:

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

Ve komponentě ```Basket``` potom při inicializaci zaregistrujeme metodu ```ProductAdded``` k eventu pomocí příkazu ```ProductService.ProductAdded += ProductAdded``` a také implementujeme rozhraní ```IDisposable``` a v metodě ```Dispose``` metodu ```ProductAdded``` odregistrujeme ```ProductService.ProductAdded -= ProductAdded```:

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

---
1. [Need your Blazor sibling components to talk to each other?](https://jonhilton.net/blazor-sibling-communication/)