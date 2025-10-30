using Microsoft.EntityFrameworkCore;

namespace Knihovna.Data
{
    public class KnihovnaDbContext(DbContextOptions<KnihovnaDbContext> options) : DbContext(options)
    {
        public DbSet<Autor> Autori { get; set; }
        public DbSet<Kniha> Knihy { get; set; }
    }
}
