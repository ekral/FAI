using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Students.WebApi.Data;
using Students.WebApi.Models;

namespace Students.Tests
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            using StudentContext context = CreateContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Student s1 = new() { StudentId = 1, Jmeno = "Pavel", Studuje = true };
            Student s2 = new() { StudentId = 2, Jmeno = "Jiri", Studuje = false };
            Student s3 = new() { StudentId = 3, Jmeno = "Jitka", Studuje = true };

            context.AddRange(s1, s2, s3);

            context.SaveChanges();
        }

        public StudentContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<StudentContext>()
                .UseSqlite("DataSource=test.db")
                .Options;

            return new StudentContext(options);
        }
    }

    public class UnitTestStudenti : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture Fixture { get; }
        public UnitTestStudenti(DatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task GetAllStudents_ShouldReturnAllStudents()
        {
            // Arrange
            using StudentContext context = Fixture.CreateContext();
            // Act
            var result = await WebApi.WebApi.GetAllStudents(context);
            // Assert
            Assert.Equal(3, result.Value?.Length);
        }

        [Fact]
        public async Task GetStudent_ShouldReturnStudent_WhenExists()
        {
            // Arrange
            using StudentContext context = Fixture.CreateContext();
            // Act
            var result = await WebApi.WebApi.GetStudentById(context, 1);
            // Assert
            Ok<Student> okStudent = Assert.IsType<Ok<Student>>(result.Result);
            Assert.Equal(1, okStudent.Value?.StudentId);
        }

        [Fact]
        public async Task GetStudent_ShouldReturnNotFound_WhenStudentDoesNotExists()
        {
            // Arrange
            using StudentContext context = Fixture.CreateContext();
            // Act
            var result = await WebApi.WebApi.GetStudentById(context, 999);
            // Assert
            Assert.IsType<NotFound>(result.Result);
        }
    }
}
