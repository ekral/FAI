using Microsoft.EntityFrameworkCore;

namespace UTB.ConcurrencyTest.WebApi
{
    class MenuContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .Property(x => x.Version)
                .IsRowVersion();
        }
    }
}
