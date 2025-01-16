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


            //int id = 1;

            //Student? student = context.Students.Find(id);

            //if (student is not null)
            //{
            //    Console.WriteLine($"{student.Id} {student.Jmeno} {student.Prijmeni}");
            //}


            IQueryable<Student> students = context.Students.Where(s => s.Prijmeni == "Vesely");

            foreach(Student student in students)
            {
                Console.WriteLine($"{student.Id} {student.Jmeno} {student.Prijmeni}");
            }
        }
    }
}
