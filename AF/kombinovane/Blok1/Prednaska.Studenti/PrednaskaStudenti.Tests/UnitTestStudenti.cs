using Prednaska.Studenti;

namespace PrednaskaStudenti.Tests
{
    public class UnitTestStudenti : IClassFixture<TestDatabaseFixture>
    {
        public TestDatabaseFixture Fixture { get; }

        public UnitTestStudenti(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }


        [Fact]
        public void StudentJeBohumil()
        {
            using StudentContext context = Fixture.CreateContext();

            Student student = context.Studenti.Single(s => s.Id == 1);

            Assert.Equal("Bohumil", student.Jmeno);
        }

        [Fact]
        public void FindStudentWithIdAndGroupName()
        {
            using StudentContext context = Fixture.CreateContext();

            Student student = context.Studenti.Single(s => s.Id == 1 && s.Skupina.Nazev == "swi");

            Assert.Equal("Bohumil", student.Jmeno);
        }

        [Fact]
        public void StudentEnrollmentInGymClass()
        {
            using StudentContext context = Fixture.CreateContext();

            bool jeZapsany = context.Studenti.Any(s => s.Jmeno == "Bohumil" && s.Predmety.Any(p => p.Nazev == "Telocvik"));

            Assert.True(jeZapsany);
        }
    }
}