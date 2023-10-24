﻿namespace Utb.Studenti.Models
{

    public class Skupina
    {
        public int Id { get; set; }
        public required string Nazev { get; set; }

        public ICollection<Student> Studenti { get; set; } = new List<Student>();
    }
}