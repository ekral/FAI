using Menza.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menza.AvaloniaClient
{
    internal class MenzaServiceMock : IMenzaService
    {

        public Task<IReadOnlyList<Jidlo>> GetJidlaAsync()
        {

            IReadOnlyList<Jidlo> jidla = new List<Jidlo>
            {
                new Jidlo() { Id = 1, Nazev = "Šunkofleky", Cena = 84.1},
                new Jidlo() { Id = 1, Nazev = "Jelitový a jitrnicový prejt", Cena = 92.3},
                new Jidlo() { Id = 1, Nazev = "Cuketové medailonky", Cena = 76.4}
            };

            Task<IReadOnlyList<Jidlo>> result = Task.FromResult(jidla);

            return result;
        }
    }
}
