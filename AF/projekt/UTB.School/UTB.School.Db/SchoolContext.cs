using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UTB.School.Db
{
    public class SchoolContext(DbContextOptions<SchoolContext> options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
    }
}
