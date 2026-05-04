using Microsoft.EntityFrameworkCore;

namespace UTB.PublicLibrary.Db
{
    public class PublicLibraryContext(DbContextOptions<PublicLibraryContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
    }
}