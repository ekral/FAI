namespace UTB.PublicLibrary.Db
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required bool IsArchived { get; set; }
    }
}