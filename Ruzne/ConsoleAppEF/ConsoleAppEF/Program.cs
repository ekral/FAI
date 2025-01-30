using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace ConsoleAppEF
{
    internal class Program
    {
        static void Zaklady()
        {
            using StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                                    .UseSqlite("Data Source=studenti.db")
                                                    .Options);

            context.Database.EnsureCreated();

            Student novy = new Student() { Jmeno = "Jiri", Prijmeni = "Vesely" };

            context.Add(novy);

            int number = context.SaveChanges();

            Console.WriteLine($"Pocet entit zapsanych do databaze: {number}");

            Console.WriteLine($"Vygenerovane Id: {novy.StudentId}");

            Student studentUpdate = new Student() { StudentId = 1, Jmeno = "Dusan", Prijmeni = "Jiny" };
            context.Studenti.Update(studentUpdate);

            context.SaveChanges();

            foreach (Student student in context.Studenti)
            {
                Console.WriteLine($"{student.StudentId} {student.Jmeno} {student.Prijmeni}");
            }

            List<Student> studentiList = context.Studenti.ToList();
            Student[] studentiArray = context.Studenti.ToArray();

            int id = 1;

            Student? studentById = context.Studenti.Find(id);

            if (studentById is not null)
            {
                Console.WriteLine($"{studentById.StudentId} {studentById.Jmeno} {studentById.Prijmeni}");
            }

            Student? studentByPrijmeni = context.Studenti.FirstOrDefault(s => s.Prijmeni.StartsWith("Nov"));

            if (studentByPrijmeni is not null)
            {
                Console.WriteLine($"{studentByPrijmeni.StudentId} {studentByPrijmeni.Jmeno} {studentByPrijmeni.Prijmeni}");
            }

            IQueryable<Student> students = context.Studenti.Where(s => s.Prijmeni == "Vesely");

            foreach (Student student in students)
            {
                Console.WriteLine($"{student.StudentId} {student.Jmeno} {student.Prijmeni}");
            }

            IQueryable<string> jmena = context.Studenti.Select(s => s.Jmeno);

            foreach (string jmeno in jmena)
            {
                Console.WriteLine(jmeno);
            }

            IOrderedQueryable<Student> serazeniStudentiDleKliceVzestupne = context.Studenti.Order();
            IOrderedQueryable<Student> serazeniStudentiDleKliceSestupne = context.Studenti.OrderDescending();
            IOrderedQueryable<Student> serazeniStudentiPodlePrijmeniVzestupne = context.Studenti.OrderBy(s => s.Prijmeni);

            var serazenaJmena = context.Studenti
                .Where(s => s.Prijmeni == "Vesely")
                .Select(s => s.Jmeno)
                .OrderDescending();
        }

        static void Relace()
        {
            using StudentContext context = CreateContext();

            Skupina skupina = new Skupina() { SkupinaId = 1, Nazev = "SWI1" };
            Student student = new Student() { StudentId = 1, SkupinaId = 1, Jmeno = "Jiri", Prijmeni = "Pokorny" };

            context.Skupiny.Add(skupina);
            context.Studenti.Add(student);

            context.SaveChanges();
        }

        static StudentContext CreateContext()
        {
            StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                                    .UseSqlite("Data Source=studenti.db")
                                                    .Options);
            return context;
        }
        static void Main(string[] args)
        {
            using StudentContext context = CreateContext();

            context.Database.EnsureCreated();

            Relace();

        }
    }
}
