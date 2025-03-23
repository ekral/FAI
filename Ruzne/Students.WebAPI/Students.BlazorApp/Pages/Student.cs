using System.ComponentModel.DataAnnotations;

namespace Students.BlazorApp.Pages
{
    public class Student
    {
        
        public int StudentId { get; set; }

        [Required]
        public required string Jmeno { get; set; }
        public required bool Studuje { get; set; }
    }
}