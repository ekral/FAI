@page "/hypoteka"

<PageTitle>Hypotéka</PageTitle>

<h1>Hypoteční kalkulačka</h1>

<div class="mb-3">
    <label class="form-label" for="pujcka">Výše půjčky</label>
    <input class="form-control" id="pujcka" type="number" step="200000" min="200000" @bind-value="pujcka" @bind-value:after="SpocitejSplatku" />
</div>

@* 2. Pridat input pro urokProcenta a pocetLet *@

<p role="status">Splátka: @splatka.ToString("F1")</p>

<h2>Splátkový kalendář</h2>

@* 3. Zobrazit splatkovy kalendar *@

<div class="form-check form-switch">
    <InputCheckbox class="form-check-input" type="checkbox" role="switch" @bind-Value="zobrazitRoky" id="roky" />
    <label class="form-check-label" for="roky">
        Zobrazit po letech
    </label>
</div>

@if (zobrazitRoky)
{
    <p>Zobrazuji po letech</p>
}
else
{
    <p>Zobrazuji po mesicich</p>
}

@code {
    private double pujcka = 6000000.0;
    private double urokProcenta = 4.0;
    private int pocetLet = 30;
    private double splatka;
    double r;
    int n;
    bool zobrazitRoky = true;

    protected override void OnInitialized()
    {
        SpocitejSplatku();
    }

    private void SpocitejSplatku()
    {
        r = urokProcenta / 12.0 / 100.0;
        n = pocetLet * 12;

        // 1. Spocitani splatky podle vzorce

    }
}
