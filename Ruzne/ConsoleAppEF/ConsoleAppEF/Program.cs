using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

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

            Student studentUpdate = new Student() { Id = 1, Jmeno = "Dusan", Prijmeni = "Jiny" };
            context.Students.Update(studentUpdate);

            context.SaveChanges();

            foreach (Student student in context.Students)
            {
                Console.WriteLine($"{student.Id} {student.Jmeno} {student.Prijmeni}");
            }

            List<Student> studentiList = context.Students.ToList();
            Student[] studentiArray = context.Students.ToArray();

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

            IQueryable<string> jmena = context.Students.Select(s => s.Jmeno);

            foreach (string jmeno in jmena)
            {
                Console.WriteLine(jmeno);
            }

            IOrderedQueryable<Student> serazeniStudentiDleKliceVzestupne = context.Students.Order();
            IOrderedQueryable<Student> serazeniStudentiDleKliceSestupne = context.Students.OrderDescending();
            IOrderedQueryable<Student> serazeniStudentiPodlePrijmeniVzestupne = context.Students.OrderBy(s => s.Prijmeni);

            var serazenaJmena = context.Students
                .Where(s => s.Prijmeni == "Vesely")
                .Select(s => s.Jmeno)
                .OrderDescending();

        }
    }
}
