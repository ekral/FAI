using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Studenti.WebAPI;

namespace Studenti.Testy
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            using StudentContext context = CreateContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Studenti.AddRange(
                new Student() { StudentId = 1, Jmeno = "Adam" },
                new Student() { StudentId = 2, Jmeno = "Jakub" },
                new Student() { StudentId = 3, Jmeno = "Petr" }
                );

            context.SaveChanges();
        }

        public StudentContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<StudentContext>()
                .UseSqlite("DataSource=testovaci.db")
                .Options;

            return new StudentContext(options);
        }
    }
    public class Studenti(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture Fixture { get; } = fixture;

        [Fact]
        public async Task VratVsechnyStudenty_VratiVsechnyStudenty()
        {
            // Arrange
            using StudentContext context = Fixture.CreateContext();

            // Act
            Ok<Student[]> studenti = await WebAPI.WebAPI.VratVsechnyStudenty(context);

            // Assert
            Assert.Equal(3, studenti.Value?.Length);
        }

        [Fact]
        public async Task VratStudenta_VratiStudenta()
        {
            // Arrange
            using StudentContext context = Fixture.CreateContext();

            // Act
            Results<Ok<Student>, NotFound> result = await WebAPI.WebAPI.VratStudentaPodleId(1, context);

            // Assert
            Ok<Student> student = Assert.IsType<Ok<Student>>(result.Result);
            Assert.NotNull(student.Value);
            Assert.Equal(1, student.Value.StudentId);
            Assert.Equal("Adam", student.Value.Jmeno);
        }

        [Fact]
        public async Task VratNeexistujiciStudenta_VratiNotFound()
        {
            // Arrange
            using StudentContext context = Fixture.CreateContext();

            // Act
            Results<Ok<Student>, NotFound> result = await WebAPI.WebAPI.VratStudentaPodleId(999, context);

            // Assert
            Assert.IsType<NotFound>(result.Result);
        }

        [Fact]
        public async Task VlozNovehoStudenta_VloziNovehoStudenta()
        {
            // Arrange
            using StudentContext context = Fixture.CreateContext();
            using var transaction = context.Database.BeginTransaction(); // transakci nepotvrdim, takze se zmeny v databazi odstrani

            // Act
            Created<Student> result = await WebAPI.WebAPI.VlozNovehoStudenta(new Student() { Jmeno = "Novy" }, context);
            context.ChangeTracker.Clear(); // zajistim, ze vrati objekt z databaze 

            // Assert
            Assert.Equal(4, context.Studenti.Count());
            Student? student = context.Studenti.Find(4);
            Assert.NotNull(student);
            Assert.Equal("Novy", student.Jmeno);
        }

       
    }
}
