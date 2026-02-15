using Microsoft.EntityFrameworkCore;

namespace UTB.Library.Db
{
    public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
    {
        public DbSet<Author> Authors { get; set; }
    }
}
