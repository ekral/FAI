namespace Csharp8Test
{
    using System.Collections.Immutable;
    using Grade = (string x, double y);
    internal class Program
    {
        static void Main(string[] args)
        {
            Grade grade = Get();
            Console.WriteLine($"{grade.x} {grade.y}");

            Student student1 = new Student("Pavel", 10);
            Zaznam zaznam = new Zaznam("Karel", 20);

            Console.WriteLine($"{student1.Jmeno} {student1.Body}");
            Console.WriteLine($"{zaznam.Jmeno} {zaznam.Body}");

            int[] pole1 = [];
            ImmutableArray<int> pole2 = [1, 2, 3];
            ImmutableList<int> list1 = [];
            ImmutableList<int> list2 = [1, 2, 3];
            IEnumerable<int> ints1 = [1, 2, 3];
            IEnumerable<int> ints2 = [1, 2, 3, .. ints1, .. list2];

            double[] znamky = [1.0, 2.0, 3.0];

            double znamka = znamky switch
            {
                [] => 0.0,
                [double jednaZnamka] => jednaZnamka,
                [..double[] all] => all.Average()
            };
        }

        static Grade Get()
        {
            return ("A", 100.0);
        }
    }

    class Student(string jmeno, int body)
    {
        public string Jmeno { get; set; } = jmeno;
        public int Body { get; set; } = body;
    }

    record Zaznam(string Jmeno, int Body);
}
