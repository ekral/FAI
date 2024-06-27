# Formuláře

```razor
@using System.ComponentModel.DataAnnotations
<h3>Bmi Calculator</h3>

<EditForm FormName="BmiForm" Model="Data" OnValidSubmit="Submit" Enhance>
    <DataAnnotationsValidator />
    <div>
        <label>
            Height:
            <br/>
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
        [Range(10.0, 300.0, ErrorMessage = "Accommodation invalid (10-300).")]
        public double Height { get; set; } = 180.0;

        [Range(1.0, 500.0, ErrorMessage = "Accommodation invalid (1-500).")]
        public double Mass { get; set; } = 75.0;
    }
}
```
---
[ASP.NET Core Blazor forms overview](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/?view=aspnetcore-8.0)
[ASP.NET Core Blazor input components](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/input-components?view=aspnetcore-8.0)