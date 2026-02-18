using System;
using System.Collections.Generic;
using System.Text;

namespace UTB.Library.Db
{
    public class Loan
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public required DateOnly Date { get; set; }
        public required LoanStatus LoanStatus { get; set; }
    }
}
