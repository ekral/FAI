using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
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

                        // add test only data
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public PizzaContext CreateContext() => new PizzaContext("test_pizzy.db");
    }
}