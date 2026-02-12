using Studenti.WebAPI.Models;

namespace Studenti.WebAPI.DTOs
{
    public record PaginationResult(Student[] Students, int Total);
}
