namespace TestProjectDatabaseFixture
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
          
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    [Collection("Database collection")]
    public class UnitTest1
    {
        DatabaseFixture fixture;

        public UnitTest1(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void Test2()
        {

        }
    }

    [Collection("Database collection")]
    public class UnitTest2
    {
        DatabaseFixture fixture;

        public UnitTest2(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void Test2()
        {

        }
    }
}