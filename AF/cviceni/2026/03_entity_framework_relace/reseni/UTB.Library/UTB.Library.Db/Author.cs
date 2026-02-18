namespace UTB.Library.Db
{
    public class Author
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Book> Books { get; set; } = [];
    }
}
