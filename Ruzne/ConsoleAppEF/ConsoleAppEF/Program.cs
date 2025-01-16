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

            Student novy = new Student() { Jmeno = "Jiri", Prijmeni = "Vesely" };

            context.Add(novy);

            int number = context.SaveChanges();

            Console.WriteLine($"Pocet entit zapsanych do databaze: {number}");
            
            Console.WriteLine($"Vygenerovane Id: {novy.Id}");

        }
    }
}
