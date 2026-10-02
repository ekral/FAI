using System.ComponentModel.DataAnnotations;

namespace UTB.Library.Web.FormModels
{
    public class AuthorFormData
    {
        [Required]
        public string? Name { get; set; }
    }
}
