// .NET 7

using System.Text.Json;

Student student = new Student() { Id = 1, Jmeno = "Petr", Skupina = "swi1" };

// JSON - citelne pro cloveka, pomalejsi, je doma v javascriptu, drive XML
// gRPC - vysoky vykon, binarne, necitelne pro cloveka

string json = JsonSerializer.Serialize(student);
Console.WriteLine(json);

Student? deserializovany = JsonSerializer.Deserialize<Student>("""{"Id":2,"Jmeno":"Ondrej","Skupina":"swi2"}""");
if (deserializovany is not null)
{
    Console.WriteLine($"{deserializovany.Id} {deserializovany.Jmeno} {deserializovany.Skupina}");
}


class Student
{
    public int Id { get; set; }
    public string Jmeno { get; set; }
    public string Skupina { get; set; }
}
