using Prednaska.Studenti;

namespace PrednaskaStudenti.Tests
{
    public class TestDatabaseFixture
    {
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public StudentContext CreateContext() => new StudentContext("test.db");
    }
}