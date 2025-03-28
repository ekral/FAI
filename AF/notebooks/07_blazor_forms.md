# 07 Blazor Web Forms

**autor: Erik Král ekral@utb.cz**

---

Data zadáváme pomocí formulářů, kdy můžeme použít jak HTML prvky, tak Blazor componenty což je běžnější. 

V následujícím příkladu je ukázka formuláře pro výpočet BMI indexu s využitím Blazor componentů. První komponentou je ```EditForm```, který má atributy ```FormName```tedy název formuláře, ```Model``` což je název property představující data formuláře a ```Submit``` jehož hodnotou je název metody, která se má zavolat na serveru pro obsluhu daného formuláře. Máme na vyýber, jestli zvolíme `OnValidSubmit`, `OnInvalidSubmit` nebo `Submit`. `Submit` se zavolá vždy a `OnValidSubmit` pouze pokud je formulář validní, viz validace níže.

Property představující data formuláře musí být označená atributem ```[SupplyParameterFromForm]```.

```EditForm``` potom obsahuje komponenty pro jednotlivé pole formuláře. Konkrétně dvě komponenty ```InputNumber```, které mají atribut ```@bind-Value="Data.Height"``` respektive ```@bind-Value="Data.Mass"``` představující obousměrné bindování na property. Znamená to, že se data jak zobrazují tak i mění.

Atribut ```Enhance``` zlepšuje uživatelský zážitek tak, že při odeslání formuláře nedojde k obnovení celé stránky ale pouze její části.

```razor
<h3>Bmi Calculator</h3>

<EditForm FormName="BmiForm" Model="Data" Submit="Submit" Enhance>
    <div>
        <label>
            Height:
            <br/>
            <InputNumber @bind-Value="Data.Height" />
        </label>
    </div>
    <div>
        <label>
            Mass:
            <br />
            <InputNumber @bind-Value="Data.Mass" />
        </label>
    </div>
    <div>
        <button type="submit">Submit</button>
    </div>
</EditForm>

Bmi: @bmi.ToString("F2")

@code {
    [SupplyParameterFromForm]
    public BmiInputData Data { get; set; } = new();

    public double bmi = 0.0;

    private void Submit()
    {
        double heightMeters = Data.Height / 100.0;

        bmi = Data.Mass / (heightMeters * heightMeters);
    }
    public class BmiInputData
    {
        public double Height { get; set; } = 180.0;
        public double Mass { get; set; } = 75.0;
    }
}
```
## Validace dat

Následující příklad představuje ukázku validace dat. Pro definování pravidel můžeme použít atributy, například atribut ```[Range(1.0, 300.0, ErrorMessage = "Height invalid (1-300).")]``` který definuje povolený rozsah hodnot pro property.

Do EditFormu potom přidáme komponentu  ```<DataAnnotationsValidator />``` a volitelně ```<ValidationSummary />``` představující seznam všech chyb při validaci. Také ale můžeme použít zápis ```<ValidationMessage For="() => Data.Height" />``` který vypíše chyby pro jednotlivé položky formuláře.

```razor
@using System.ComponentModel.DataAnnotations
<h3>Bmi Calculator</h3>

<EditForm FormName="BmiForm" Model="Data" OnValidSubmit="Submit" Enhance>
    <DataAnnotationsValidator />
    <ValidationSummary />
    <div>
        <label>
            Height:
            <br />
            <ValidationMessage For="() => Data.Height" />
            <InputNumber @bind-Value="Data.Height" />
        </label>
    </div>
    <div>
        <label>
            Mass:
            <br />
            <ValidationMessage For="() => Data.Mass" />
            <InputNumber @bind-Value="Data.Mass" />
        </label>
    </div>
    <div>
        <button type="submit">Submit</button>
    </div>
</EditForm>

Bmi: @bmi.ToString("F2")

@code {
    [SupplyParameterFromForm]
    public BmiInputData Data { get; set; } = new();

    public double bmi = 0.0;

    private void Submit()
    {
        double heightMeters = Data.Height / 100.0;

        bmi = Data.Mass / (heightMeters * heightMeters);
    }
    public class BmiInputData
    {
        [Range(1.0, 300.0, ErrorMessage = "Height invalid (1-300).")]
        public double Height { get; set; } = 180.0;

        [Range(1.0, 500.0, ErrorMessage = "Mass invalid (1-500).")]
        public double Mass { get; set; } = 75.0;
    }
}
```
---
1. [ASP.NET Core Blazor forms overview](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/?view=aspnetcore-8.0)
2. [ASP.NET Core Blazor input components](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/input-components?view=aspnetcore-8.0)