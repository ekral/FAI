using System.Net.Http.Json;
using UTB.School.Contracts;
using UTB.School.Db;

namespace UTB.School.Tests.Tests
{

    [Collection("Database collection")]
    public class StudentTests(TestFixture fixture)
    {
        private readonly TestFixture fixture = fixture;

        [Fact]
        public async Task CreateStudent_ReturnsCreatedAndPersistsStudent()
        {
            var studentRequestDto = new StudentRequestDto("Franz Kafka", true);

            var response = await fixture.HttpClient.PostAsJsonAsync("/students", studentRequestDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            StudentDto? studentDto = await response.Content.ReadFromJsonAsync<StudentDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(studentDto);
            Assert.Equal(studentRequestDto.Name, studentDto.Name);
            Assert.NotNull(response.Headers.Location);
            Assert.EndsWith($"/students/{studentDto.Id}", response.Headers.Location.ToString());

            using var context = fixture.CreateContext();

            Student? student = await context.Students.FindAsync([studentDto.Id], TestContext.Current.CancellationToken);

            Assert.NotNull(student);
            Assert.Equal(studentRequestDto.Name, student.Name);
            Assert.Equal(studentRequestDto.IsActive, student.IsActive);
        }

        [Fact]
        public async Task GetStudents_ReturnsAllStudents()
        {
            var response = await fixture.HttpClient.GetAsync("/students", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            StudentDto[]? students = await response.Content.ReadFromJsonAsync<StudentDto[]>(TestContext.Current.CancellationToken);

            Assert.NotNull(students);
            Assert.True(students.Length > 2);
            Assert.Contains(students, a => a.Name == "Jan" && a.IsActive == true);
            Assert.Contains(students, a => a.Name == "Eva" && a.IsActive == true);
            Assert.Contains(students, a => a.Name == "Petr" && a.IsActive == false);
        }

        [Fact]
        public async Task GetStudentById_ReturnsOkAndStudent_WhenStudentExists()
        {
            var response = await fixture.HttpClient.GetAsync("/students/1", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            StudentDto? studentDto = await response.Content.ReadFromJsonAsync<StudentDto>(TestContext.Current.CancellationToken);

            Assert.NotNull(studentDto);
            Assert.Equal("Jan", studentDto.Name);
            Assert.True(studentDto.IsActive);
        }

        [Fact]
        public async Task GetStudentById_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            var response = await fixture.HttpClient.GetAsync("/students/999", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteStudent_DeletesAndReturnsNoContent_WhenStudentExists()
        {
            var tereza = new Student { Name = "Tereza", IsActive = true };

            using (var context = fixture.CreateContext())
            {
                context.Students.Add(tereza);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            var response = await fixture.HttpClient.DeleteAsync($"/students/{tereza.Id}", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            using (var context = fixture.CreateContext())
            {
                var student = await context.Students.FindAsync([tereza.Id], TestContext.Current.CancellationToken);

                Assert.Null(student);
            }
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContentAndUpdatesBook()
        {
            var josef = new Student { Name = "Josef", IsActive = true };

            using (var context = fixture.CreateContext())
            {
                context.Students.Add(josef);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            StudentRequestDto studentRequestDto = new("Pepa", false);

            var response = await fixture.HttpClient.PutAsJsonAsync($"/students/{josef.Id}", studentRequestDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            using (var context = fixture.CreateContext())
            {
                var student = await context.Students.FindAsync([josef.Id], TestContext.Current.CancellationToken);

                Assert.NotNull(student);
                Assert.Equal(studentRequestDto.Name, student.Name);
                Assert.Equal(studentRequestDto.IsActive, student.IsActive);
            }
        }

        [Fact]
        public async Task PatchBookArchiveState_ReturnsNoContentAndUpdatesFlag()
        {
            var josef = new Student { Name = "Josef", IsActive = true };

            using (var context = fixture.CreateContext())
            {
                context.Students.Add(josef);

                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            StudentPatchRequestDto studentPatchRequestDto = new(false);

            var response = await fixture.HttpClient.PutAsJsonAsync($"/students/{josef.Id}", studentPatchRequestDto, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            using (var context = fixture.CreateContext())
            {
                var student = await context.Students.FindAsync([josef.Id], TestContext.Current.CancellationToken);

                Assert.NotNull(student);
                Assert.Equal(studentPatchRequestDto.Name, student.Name);
                Assert.Equal(studentPatchRequestDto.IsActive, student.IsActive);
            }
        }
    }
}
