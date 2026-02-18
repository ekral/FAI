using System;
using System.Collections.Generic;
using System.Text;

namespace UTB.Library.Db
{
    public class Book
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Author> Authors { get; set; } = [];
        public Loan? Loan { get; set; }
    }
}
