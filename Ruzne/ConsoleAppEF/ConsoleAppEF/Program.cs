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

        static void Relace1()
        {
            using StudentContext context = CreateContext();

          
            var skupiny = context.Skupiny;

            foreach(Skupina skupina in skupiny)
            {
                // skupina.Studenti bude null
            }

            var skupinySeStudenty = context.Skupiny.Include(skupina => skupina.Studenti);

            foreach (Skupina skupina in skupiny)
            {
                Console.WriteLine($"Skupina {skupina.SkupinaId}: {skupina.Nazev}");

                if (skupina.Studenti is not null)
                {
                    foreach (Student student in skupina.Studenti)
                    {
                        Console.WriteLine($"Student {student.StudentId}: {student.Jmeno} {student.Prijmeni}");
                    }
                }
            }


            

        }

        static void Relace2()
        {
            using StudentContext context = CreateContext();

            Skupina skupina = context.Skupiny.Single(s => s.SkupinaId == 1);

            if (skupina.Studenti is null)
            {
                Console.WriteLine("Studenti jsou zatím null.");
            }

            context.Entry(skupina).Collection(skupina => skupina.Studenti).Load();

            if (skupina.Studenti is not null)
            {
                foreach (Student student in skupina.Studenti)
                {
                    Console.WriteLine($"Student {student.StudentId}: {student.Jmeno} {student.Prijmeni}");
                }
            }
        }

        static void Relace3()
        {
            using StudentContext context = CreateContext();

            Student student = context.Studenti.Single(student => student.StudentId == 1);

            if(student.Skupina is null)
            {
                Console.WriteLine("Skupina je zatím null.");
            }

            context.Entry(student).Reference(student => student.Skupina).Load();

            if (student.Skupina is not null)
            {
                Console.WriteLine($"Skupina {student.Skupina.SkupinaId}: {student.Skupina.Nazev}");
            }
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

            if (context.Database.EnsureCreated())
            {
                Skupina skupina1 = new Skupina() { SkupinaId = 1, Nazev = "SWI1" };
                Student student1 = new Student() { StudentId = 1, SkupinaId = 1, Jmeno = "Jiri", Prijmeni = "Pokorny" };
                Student student2 = new Student() { StudentId = 2, SkupinaId = 1, Jmeno = "Alena", Prijmeni = "Dulikova" };

                context.Skupiny.Add(skupina1);
                context.Studenti.AddRange(student1, student2);

                context.SaveChanges();
            }

            Relace2();
            Relace3();

        }
    }
}
