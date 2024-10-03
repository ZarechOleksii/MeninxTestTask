using DAL;
using DAL.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Tests.DAL
{
    [TestClass]
    public class BookRepositoryTests
    {
        private readonly LibraryDBContext _libraryDBContext;
        private readonly Mock<ILogger<BookRepository>> _loggerMock;
        private readonly BookRepository _rep;

        private Book GetSampleBook()
        {
            return new Book()
            {
                Title = "title",
                Author = "A",
                PublicationYear = 2010,
                Quantity = 10,
                ISBN = "978-617-7171-80-4"
            };
        }

        private IEnumerable<Book> GetSampleBooks(int n)
        {
            List<Book> books = new List<Book>();
            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                books.Add(new Book()
                {
                    Title = "Name" + rand.Next(),
                    Author = "A" + rand.Next(),
                    ISBN = "978-617-" + rand.Next(1000, 10000) + "-80-4",
                    PublicationYear = rand.Next(2000, 2030),
                    Quantity = rand.Next(),
                });
            }

            return books;
        }

        public BookRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDBContext>()
                .UseInMemoryDatabase("Meninx")
                .Options;

            _libraryDBContext = new LibraryDBContext(options);
            _loggerMock = new Mock<ILogger<BookRepository>>();
            _rep = new BookRepository(_libraryDBContext, _loggerMock.Object);
        }

        [TestMethod]
        public void GetOne_NotFound_ReturnsNull()
        {
            //act
            var result = _rep.GetOne(Guid.NewGuid());

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetOne_Found_ReturnsObject()
        {
            //arrange
            const string title = "name";
            Guid bookId = Guid.NewGuid();
            Book sample = GetSampleBook();
            sample.Title = title;
            sample.Id = bookId;
            _libraryDBContext.Books.Add(sample);
            _libraryDBContext.SaveChanges();

            //act
            var result = _rep.GetOne(bookId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(title, result.Title);
        }

        [TestMethod]
        public async Task GetOneAsync_NotFound_ReturnsNull()
        {
            //act
            var result = await _rep.GetOneAsync(Guid.NewGuid());

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetOneAsync_Found_ReturnsObject()
        {
            //arrange
            const string title = "name";
            Guid bookId = Guid.NewGuid();
            Book sample = GetSampleBook();
            sample.Title = title;
            sample.Id = bookId;
            _libraryDBContext.Books.Add(sample);
            _libraryDBContext.SaveChanges();

            //act
            var result = await _rep.GetOneAsync(bookId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(title, result.Title);
        }

        [TestMethod]
        public async Task BookExistsAsync_NotFound_ReturnsFalse()
        {
            //act
            var result = await _rep.BookExistsAsync(Guid.NewGuid());

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BookExistsAsync_Found_ReturnsTrue()
        {
            //arrange
            Guid bookId = Guid.NewGuid();
            Book sample = GetSampleBook();
            sample.Id = bookId;
            _libraryDBContext.Books.Add(sample);
            _libraryDBContext.SaveChanges();

            //act
            var result = await _rep.BookExistsAsync(bookId);

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetOneWithCategory_NotFound_ReturnsNull()
        {
            //act
            var result = _rep.GetOneWithCategory(Guid.NewGuid());

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetOneWithCategory_Found_GetsWithCategory()
        {
            //arrange
            Guid categoryId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId, Name = "name", Description = "description" };
            _libraryDBContext.Categories.Add(category);

            Book sample = GetSampleBook();
            sample.Id = bookId;
            sample.Category = category;
            sample.CategoryId = categoryId;

            _libraryDBContext.Books.Add(sample);
            _libraryDBContext.SaveChanges();

            //act
            var result = _rep.GetOneWithCategory(bookId);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Category);
            Assert.AreEqual(categoryId, result.CategoryId);
            Assert.AreEqual(categoryId, result.Category.Id);
        }

        [TestMethod]
        public void IsISBNAvailable_Found_ReturnsFalse()
        {
            //arrange
            const string ISBN = "978-617-7171-80-4";
            Guid bookId = Guid.NewGuid();
            Book sample = GetSampleBook();
            sample.Id = bookId;
            sample.ISBN = ISBN;
            _libraryDBContext.Books.Add(sample);
            _libraryDBContext.SaveChanges();

            //act
            var result = _rep.IsISBNAvailable(ISBN);

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsISBNAvailable_NotFound_ReturnsTrue()
        {
            //act
            var result = _rep.IsISBNAvailable("978-617-7171-80-4");

            //assert
            Assert.IsTrue(result);
        }

        /* This throws NotImplemented exception, I assume testing procedures with InMemoryDb is not feasible
        [TestMethod]
        public async Task GetAllAsync_TestOrdering_WorksLikeLinq()
        {
            //arrange
            const int n = 50;
            IEnumerable<Book> books = GetSampleBooks(n);
            _libraryDBContext.AddRange(books);
            _libraryDBContext.SaveChanges();

            //act
            List<Book> result = (await _rep.GetAllAsync(n, 0, "", "Title", "")).ToList();
            List<Book> linqResult = books.OrderBy(q => q.Title).Skip(0).Take(n).ToList();

            //assert
            for(int i = 0; i <= n; i++)
            {
                Assert.AreEqual(linqResult[i].Id, result[i].Id);
            }
        }
        */
    }
}
