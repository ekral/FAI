using Knihovna.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.WebAPI.Data
{
    public class KnihovnaContext(DbContextOptions<KnihovnaContext> options) : DbContext(options)
    {
        public DbSet<Ctenar> Ctenari { get; set; }
        public DbSet<Kniha> Knihy { get; set; }
        public DbSet<Vypujcka> Vypujcky { get; set; }
    }
}
