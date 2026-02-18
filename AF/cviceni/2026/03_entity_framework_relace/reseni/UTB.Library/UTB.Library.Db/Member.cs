using System;
using System.Collections.Generic;
using System.Text;

namespace UTB.Library.Db
{
    public class Member
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Loan> Loans { get; set; } = [];
    }
}
