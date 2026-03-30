using Aspire.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using UTB.School.Contracts;
using UTB.School.Db;

namespace UTB.School.Tests.Tests
{
    public class TestFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;
        private string? connectionString;
        public HttpClient HttpClient { get; private set; } = null!;

        public async ValueTask InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UTB_School_AppHost>(["--environment=Testing"], TestContext.Current.CancellationToken);

            app = await builder.BuildAsync(TestContext.Current.CancellationToken);

            await app.StartAsync(TestContext.Current.CancellationToken);

            await app.ResourceNotifications.WaitForResourceHealthyAsync("database", TestContext.Current.CancellationToken);
            await app.ResourceNotifications.WaitForResourceHealthyAsync("webapi", TestContext.Current.CancellationToken);

            connectionString = await app.GetConnectionStringAsync("database", TestContext.Current.CancellationToken);
            HttpClient = app.CreateHttpClient("webapi", "https");

            using var context = CreateContext();

            await context.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
            await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

            Student jan = new() { Name = "Jan", IsActive = true };
            Student eva = new() { Name = "Eva", IsActive = true };
            Student petr = new() { Name = "Petr", IsActive = false };

            context.Students.AddRange(jan, eva, petr);

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            HttpClient.Dispose();
            await app.DisposeAsync();

            GC.SuppressFinalize(this);
        }

        public SchoolContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                    .UseNpgsql(connectionString)
                    .Options;

            var context = new SchoolContext(options);

            return context;
        }
    }

    [CollectionDefinition("Database collection", DisableParallelization = true)]
    public class DatabaseCollection : ICollectionFixture<TestFixture>
    {
    }

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

                var response = await fixture.HttpClient.DeleteAsync($"/students/{tereza.Id}", TestContext.Current.CancellationToken);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            using (var context = fixture.CreateContext())
            {
                var studentDto = await context.Students.FindAsync([tereza.Id], TestContext.Current.CancellationToken);

                Assert.Null(studentDto);
            }
        }
    }
}
