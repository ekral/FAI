namespace UTB.School.Db
{
    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required bool IsActive { get; set; }
    }
}
