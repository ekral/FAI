using Knihovna.WebAPI;
using Knihovna.WebAPI.Data;
using Knihovna.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database

namespace Knihovna.Tests
{
    public class DatabaseFixture
    {
        private static readonly Lock _lock = new();
        private static bool _databaseInitialized = false;

        public DatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using KnihovnaContext context = CreateContext();

                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    context.Knihy.AddRange(
                        new Kniha() { KnihaId = 1, Nazev = "Svejk" },
                        new Kniha() { KnihaId = 2, Nazev = "Temno" },
                        new Kniha() { KnihaId = 3, Nazev = "Pan prstenu" }
                        );

                    context.Ctenari.AddRange(
                        new Ctenar() { CtenarId = 1, Jmeno = "Petr" },
                        new Ctenar() { CtenarId = 2, Jmeno = "Erik" },
                        new Ctenar() { CtenarId = 3, Jmeno = "Alena" }
                        );

                    context.SaveChanges();

                    _databaseInitialized = true;
                }
            }
        }

        public KnihovnaContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<KnihovnaContext>()
                .UseSqlite("DataSource=test.db")
                .Options;

            return new KnihovnaContext(options);
        }
    }

    public class UnitTestWebAPI(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture Fixture { get; } = fixture;

        [Fact]
        public async Task GetAllBooks_ShouldReturnAllBooks()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();

            // Act
            var result = await WebApiVersion1.GetAllBooks(context);

            // Assert
            Assert.Equal(3, result.Value?.Length);
        }

        [Fact]
        public async Task GetBook_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();

            // Act
            var result = await WebApiVersion1.GetBook(1, context);

            // Assert
            Ok<Kniha> okKniha = Assert.IsType<Ok<Kniha>>(result.Result);

            Assert.Equal(1, okKniha.Value?.KnihaId);
        }

        [Fact]
        public async Task GetBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();

            // Act
            var result = await WebApiVersion1.GetBook(999, context);

            // Assert
            Assert.IsType<NotFound>(result.Result);
        }

        [Fact]
        public async Task CreateBook_ShouldAddBook()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();
            
            context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

            var newBook = new Kniha { KnihaId = 4, Nazev = "New Book" };

            // Act
            _ = await WebApiVersion1.CreateBook(newBook, context);

            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(4, await context.Knihy.CountAsync());
            Kniha? book = await context.Knihy.FindAsync(4);
            Assert.NotNull(book);
            Assert.Equal("New Book", book.Nazev);
        }

        [Fact]
        public async Task UpdateBook_ShouldUpdateBook_WhenBookExists()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();

            context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

            var updatedBook = new Kniha { Nazev = "Updated Book" };

            // Act
            var result = await WebApiVersion1.UpdateBook(1, updatedBook, context);

            context.ChangeTracker.Clear();

            // Assert
            Assert.IsType<NoContent>(result.Result);
            Kniha? book = await context.Knihy.FindAsync(1);
            Assert.NotNull(book);
            Assert.Equal("Updated Book", book.Nazev);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();

            context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

            var updatedBook = new Kniha { Nazev = "Updated Book" };

            // Act
            var result = await WebApiVersion1.UpdateBook(999, updatedBook, context);

            // Assert
            Assert.IsType<NotFound>(result.Result);
        }

        [Fact]
        public async Task DeleteBook_ShouldRemoveBook_WhenBookExists()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();

            context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

            // Act
            var result = await WebApiVersion1.DeleteBook(1, context);

            context.ChangeTracker.Clear();

            // Assert
            Assert.IsType<NoContent>(result.Result);
            Assert.Null(await context.Knihy.FindAsync(1));
        }

        [Fact]
        public async Task DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            using KnihovnaContext context = Fixture.CreateContext();

            context.Database.BeginTransaction(); // nechci ukladat zmeny do databaze

            // Act
            var result = await WebApiVersion1.DeleteBook(999, context);

            // Assert
            Assert.IsType<NotFound>(result.Result);
        }
    }
}