# Výpočet splátky hypotéky

Spočítejte a vypište výši měsíční spátky hypotéky dle vzorce:

$$m = \frac{p \cdot r \cdot (1 + r)^n}{(1 + r)^n - 1}$$

Kde:
- *m* je měsíční splátka hypotéky.
- *p* je výše půjčené částky (počáteční zůstatek půjčky).
- *r* je měsíční desetinná úroková míra (roční úroková míra v procentech (6%) dělená 12 měsíci (6 / 12) a převedená na desetinné číslo ( 6 / 12 / 100).
- *n* je celkový počet měsíčních plateb (doba trvání půjčky v letech násobená 12).

Pojmy:
- **Úrok**: nominální částka, například 5000,- Kč.
- **Úroková míra**:  úrok vyjádřený v procentech z částky, například 6 %.
- **Desetinná úroková míra**: úrok vyjádřený jako desetinné číslo pro výpočet v matematických operacích, například 0.06.

Dále vypište splátkový kalendář. Každý měsíc vypište výši úroku, úmoru a aktuálního dluhu:

$n-krát$ zopakujte následující kroky:
1) Nejprve spočítejte nominální výši úroku, tedy $urok = r \cdot p$
2) Úmor se potom rovná výše splátky - nominální výše úroku, tedy $umor = m - urok$
3) Snižte částku *p* o výši úmoru, tedy $p = p - umor$.
   
Výchozí kód funkce **main**:

```razor
@page "/hypoteka"

<PageTitle>Hypotéka</PageTitle>

<h1>Hypoteční kalkulačka</h1>

<div class="mb-3">
    <label class="form-label" for="vaha">Výše půjčky</label>
    <input class="form-control" type="number" step="100000" min="100000" id="vaha" @bind-value="pujcka" @bind-value:after="SpocitejSplatku" />
</div>

<p role="status">Splátka: @splatka.ToString("F1")</p>

<h2>Splátkový kalendář</h2>
@* Zobrazit splatkovy kalendar*@

@code {
    private double pujcka = 6000000.0;
    private double urokProcenta = 4.0;
    private int pocetLet = 30;
    private double splatka;
    double r;
    int n;

    protected override void OnInitialized()
    {
        SpocitejSplatku();
    }

    private void SpocitejSplatku()
    {
        r = urokProcenta / 12.0 / 100.0;
        n = pocetLet * 12;

        // Spocitani splatky podle vzorce
    }
}
```