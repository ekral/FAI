using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                                    .UseSqlite("Data Source=studenti.db")
                                                    .Options);

            context.Database.EnsureCreated();

        }
    }
}
