# Kávovar

## 🔢 Popis

Máte kávovar který má zásobník na vodu a na kávu. Pokaždé, když dáme uvařit kávu, tak ze zásobníku ubude voda a káva dle zadaných konstant. 

Program bude zobrazovat stav vody, stav kávy a zprávu zda byla káva uvařena, nebo jinou barvou že došla voda respektive káva.

## 🚀 Úkoly

1. V následujícím kódu máte neúplné řešení zadání. Doplňte správná klíčová slova k parametrům a argumentům metody `Kavovar.UvarKavu` tak aby byl program funkční.

2. Řešení je s doplněním klíčových slov funkční, ale kód lze zorganizovat lépe s lepším využitím třídy. Přepište kód tak aby lépe využíval třídu a principy objektově orientovaného programování.

3. Doplňte tlačítka a kód pro doplnění vody a kávy do kávovaru.

```razor
@page "/kavovar"

<PageTitle>Automat na kávu</PageTitle>

<h1>Automat na kávu</h1>

<p role="status">Počet káv: @pocet</p>
<p role="status">Stav vody: @stavVody ml</p>
<p role="status">Stav kávy: @stavKavy mg</p>

@if (@ok)
{
    <p role="status">Status: @message</p>
}
else
{
    <p role="alert" class="text-danger">Status: @message</p>
}

<button class="btn btn-primary" @onclick="UvarKavu">Uvař kávu</button>

@code {


    class Kavovar
    {
        private const int davkaVody = 200;
        private const int davkaKavy = 15;

        public bool UvarKavu(int stavVody, int stavKavy, string message)
        {
            if (stavVody < davkaVody)
            {
                message = "Malo vody";
                return false;
            }

            if (stavKavy < davkaKavy)
            {
                message = "Malo kavy";
                return false;
            }

            message = "Kava uvarena";

            stavVody -= davkaVody;
            stavKavy -= davkaKavy;

            return true;
        }
    }

    Kavovar kavovar = new Kavovar();

    public int pocet = 0;
    public int stavVody = 1000; // ml
    public int stavKavy = 200;  // g
    public string message = string.Empty;
    public bool ok = true;

    private void UvarKavu()
    {
        ok = kavovar.UvarKavu(stavVody, stavKavy, message);

        if(ok)
        {
            ++pocet;
        }
    }
}
```
