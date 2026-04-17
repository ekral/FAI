using System.ComponentModel.DataAnnotations;

namespace UTB.PublicLibrary.Web.FormModels
{
    public class BookFormModel
    {
        [Required]
        public string? Title { get; set; }
        public bool IsArchived { get; set; }
    }
}
