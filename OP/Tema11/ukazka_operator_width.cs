// !! Vyzaduje alespon .NET 6.0
// Ukazka pouziti operator with se strukturou
// Operator with byl predstaveny v C# 9 pro pouziti s record, ale od .NET 6 a C# 10 jej muzeme pouzit i se strukturou
// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/with-expression

Student s1;
s1.jmeno = "Jana";
s1.id = 1;

Student s2 = s1 with { id = 2 };

List<Student> studenti = new List<Student>()
{
    new Student { id = 1, jmeno  = "Pavel"},
    new Student { id = 2, jmeno  = "Alena"},
};

// Neni mozne provest, protoze se vraci kopie hodnotoveho typu (struktury) a nezmenila by se hodnota v poli
//studenti[0].jmeno = "Karel";

studenti[0] = studenti[0] with { jmeno = "Karel" };

// misto
//Student tmp = studenti[0];
//tmp.jmeno = "Karel";
//studenti[0] = tmp;

foreach (Student student in studenti)
{
    Console.WriteLine($"{student.id} {student.jmeno}");
}

struct Student
{
    public string jmeno;
    public int id;
}
