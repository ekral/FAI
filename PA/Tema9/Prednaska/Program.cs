using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

List<int> cisla = new List<int> { 1, 2, 3, 4 };
Zmen(ref cisla);
Console.WriteLine(string.Join(",", cisla));
static void Zmen(ref List<int> cisla)
{
    cisla = new List<int> { 0, 0, 0, 0 };
}


class Trida
{
    public int Id { get; set; }
    public required string Jmeno { get; set; }
}
