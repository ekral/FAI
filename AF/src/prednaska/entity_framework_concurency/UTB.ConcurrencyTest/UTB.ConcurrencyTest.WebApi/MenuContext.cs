using Microsoft.EntityFrameworkCore;

namespace UTB.ConcurrencyTest.WebApi
{
    class MenuContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Menu> Menus { get; set; }
    }
}
