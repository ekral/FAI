using Students.WebAPI.Models;

namespace Students.WebAPI.DTOs
{
    public record PaginationResult(Student[] Students, int Total);
}
