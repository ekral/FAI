using ReactiveUI;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Concurrency;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.CustomerApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Pizza? _pizza;

    public Pizza? Pizza
    {
        get => _pizza;
        set => this.RaiseAndSetIfChanged(ref _pizza, value);
    }

    public MainViewModel()
    {
        RxApp.MainThreadScheduler.Schedule(LoadPizza);
    }

    private async void LoadPizza()
    {
        Pizza? pizza = await App.Client.GetFromJsonAsync<Pizza>(@"https://localhost:7149/Pizza/2");

        if(pizza is not null)
        {
            Pizza = pizza;
        }
    }
}


