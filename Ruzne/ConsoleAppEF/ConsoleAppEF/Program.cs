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


            int id = 1;

            Student? studentById = context.Students.Find(id);

            if (studentById is not null)
            {
                Console.WriteLine($"{studentById.Id} {studentById.Jmeno} {studentById.Prijmeni}");
            }

            Student? studentByPrijmeni = context.Students.FirstOrDefault(s => s.Prijmeni.StartsWith("Nov"));

            if (studentByPrijmeni is not null)
            {
                Console.WriteLine($"{studentByPrijmeni.Id} {studentByPrijmeni.Jmeno} {studentByPrijmeni.Prijmeni}");
            }

            IQueryable<Student> students = context.Students.Where(s => s.Prijmeni == "Vesely");

            foreach(Student student in students)
            {
                Console.WriteLine($"{student.Id} {student.Jmeno} {student.Prijmeni}");
            }

            var jmena = context.Students
                .Where(s => s.Prijmeni == "Vesely")
                .Select(s => s.Jmeno)
                .OrderDescending();

        }
    }
}
