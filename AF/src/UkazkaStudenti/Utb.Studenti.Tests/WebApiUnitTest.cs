using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utb.Studenti.Models;

namespace Utb.Studenti.Tests
{
    [Collection("Database collection")]
    public class WebApiUnitTest : IClassFixture<TestDatabaseFixture>
    {
        public WebApiUnitTest(TestDatabaseFixture fixture) => Fixture = fixture;

        public TestDatabaseFixture Fixture { get; }

        [Fact]
        public async Task GetAllStudentsFromDatabase()
        {
            using StudentContext context = Fixture.CreateContext();

            Ok<Student[]> studenti = await StudentEndpointsV1.GetAllStudents(context);

            Assert.NotNull(studenti.Value);
            
            Assert.NotEmpty(studenti.Value);
            
            Assert.Collection(studenti.Value, 
                s1 =>
                {
                    Assert.Equal(1, s1.Id);
                    Assert.Equal("Bohumil", s1.Jmeno);
                }, 
                s2 =>
                {
                    Assert.Equal(2, s2.Id);
                    Assert.Equal("Stefan", s2.Jmeno);
                });
        }
    }
}
