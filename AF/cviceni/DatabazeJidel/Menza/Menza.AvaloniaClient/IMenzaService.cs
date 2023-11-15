using Menza.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menza.AvaloniaClient
{
    public interface IMenzaService
    {
        Task<IReadOnlyList<Jidlo>?> GetJidlaAsync();
    }
}
