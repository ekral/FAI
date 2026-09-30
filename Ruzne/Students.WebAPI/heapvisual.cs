using System.Collections.Generic;

List<Student> studenti = [ 
    new Student() { Jmeno = "Pavel", Vek = 21 },
    new Student() { Jmeno = "Karel", Vek = 19 },
    new Student() { Jmeno = "Alena", Vek = 20 },
];

Inspect.MemoryGraph(studenti);

class Student
{
    private string jmeno = string.Empty;
    public string Jmeno {get => jmeno; set => jmeno = value; }
    
    private int vek;
    public int Vek {get => vek; set => vek = value; }
}