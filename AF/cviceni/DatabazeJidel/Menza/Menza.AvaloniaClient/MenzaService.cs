using Avalonia.Controls;
using Menza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Menza.AvaloniaClient
{
    internal class MenzaService : IMenzaService
    {
        private readonly HttpClient httpClient;

        public MenzaService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IReadOnlyList<Jidlo>?> GetJidlaAsync()
        {

            IReadOnlyList<Jidlo>? jidla = await httpClient.GetFromJsonAsync<IReadOnlyList<Jidlo>>("");

            return jidla;
        }
    }
}
