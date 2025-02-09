using System.Text.Json;

Student student = new Student()
{
    Id = 1,
    Jmeno = "Jan Novak"
};

string jsonString = JsonSerializer.Serialize(student);

Console.WriteLine($"JSON retezec: {jsonString}");

string jsonStringStudent = """ {"Id":1,"Jmeno":"Jan Novak"} """;

Student? deserializedStudent = JsonSerializer.Deserialize<Student>(jsonStringStudent);

if (deserializedStudent is not null)
{
    Console.WriteLine($"Deserializovanz student Id: {deserializedStudent.Id}, Jmeno: {deserializedStudent.Jmeno}");
}

class Student
{
    public int Id { get; set; }
    public required string Jmeno { get; set; }
}