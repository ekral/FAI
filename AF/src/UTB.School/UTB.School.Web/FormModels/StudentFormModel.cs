using System.ComponentModel.DataAnnotations;

namespace UTB.School.Web.FormModels
{
    public class StudentFormModel
    {
        [Required]
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
