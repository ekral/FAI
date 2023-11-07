using DynamicData;
using Menza.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace Menza.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly HttpClient client = new();

        public ObservableCollection<JidloViewModel> Jidla { get; } = new();

        public MainWindowViewModel()
        {
            RxApp.MainThreadScheduler.Schedule(NactiJidla);
        }

        private async void NactiJidla()
        {
            Jidlo[]? jidla = await client.GetFromJsonAsync<Jidlo[]>("https://localhost:7007");

            if (jidla is not null)
            {
                foreach (Jidlo jidlo in jidla)
                {
                    Jidla.Add(new JidloViewModel(jidlo));
                }
            }
        }
    }
}