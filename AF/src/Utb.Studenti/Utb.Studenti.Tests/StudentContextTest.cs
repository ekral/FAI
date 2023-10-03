using Microsoft.EntityFrameworkCore;
using Utb.Studenti.Models;

namespace Utb.Studenti.Tests
{
    public class StudentContextTest : IClassFixture<TestDatabaseFixture>
    {
        public StudentContextTest(TestDatabaseFixture fixture) => Fixture = fixture;

        public TestDatabaseFixture Fixture { get; }

        [Fact]
        public void StudentJeBohumil()
        {
            using StudentContext context = Fixture.CreateContext();

            Student student = context.Studenti.Single(s => s.Id == 1);

            Assert.Equal("Bohumil", student.Jmeno);
        }

        [Fact]
        public void BohumilMaTelocvik()
        {
            using StudentContext context = Fixture.CreateContext();

            Student student = context.Studenti.First(s => s.Id == 1 && s.Predmety.Any(p => p.Nazev == "Telocvik"));

            Assert.Equal("Bohumil", student.Jmeno);
        }
    }
}