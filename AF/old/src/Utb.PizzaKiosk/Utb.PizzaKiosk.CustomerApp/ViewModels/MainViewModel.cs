using ReactiveUI;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Concurrency;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.CustomerApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private List<Pizza>? _pizzas;

    public List<Pizza>? Pizzas
    {
        get => _pizzas;
        set => this.RaiseAndSetIfChanged(ref _pizzas, value);
    }

    public MainViewModel()
    {
        RxApp.MainThreadScheduler.Schedule(LoadPizza);
    }

    private async void LoadPizza()
    {
        List<Pizza>? pizzas = await App.Client.GetFromJsonAsync<List<Pizza>>(@"https://localhost:7149/");

        if(pizzas is not null)
        {
            Pizzas = pizzas;
        }
    }
}


