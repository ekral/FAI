using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbContextOptions<StudentContext> options = new DbContextOptionsBuilder<StudentContext>()
                                                    .UseSqlite("Data Source=studenti.db")
                                                    .Options;

            using StudentContext context = new(options);

            context.Database.EnsureCreated();
        }
    }
}
