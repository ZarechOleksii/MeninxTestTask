using DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using WebForms;
using WebForms.Controllers;

namespace Tests.WebAPI
{
    [TestClass]
    public class BooksControllerTests
    {
        private readonly Mock<IBookRepository> _bookRepMock;
        private readonly BooksController _booksController;

        public BooksControllerTests()
        {
            _bookRepMock = new Mock<IBookRepository>();
            _booksController = new BooksController(_bookRepMock.Object);
        }

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

        [TestMethod]
        public async Task GetBookAsync_Found_ReturnsBook()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _bookRepMock.Setup(q => q.GetOneAsync(id)).ReturnsAsync(book);

            //act
            var result = await _booksController.GetBookAsync(id);

            //assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Book>));
            Assert.AreEqual(id, ((OkNegotiatedContentResult<Book>)result).Content.Id);
        }

        [TestMethod]
        public async Task GetBookAsync_NotFound_ReturnsBookNotFound()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = null;
            _bookRepMock.Setup(q => q.GetOneAsync(id)).ReturnsAsync(book);

            //act
            var result = await _booksController.GetBookAsync(id);

            //assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteBookAsync_NotFound_ReturnsBookNotFound()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = null;
            _bookRepMock.Setup(q => q.GetOneAsync(id)).ReturnsAsync(book);

            //act
            var result = await _booksController.DeleteBookAsync(id);

            //assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteBookAsync_DeleteSuccess_ReturnsOk()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _bookRepMock.Setup(q => q.GetOneAsync(id)).ReturnsAsync(book);
            _bookRepMock.Setup(q => q.Delete(book)).Returns(true);

            //act
            var result = await _booksController.DeleteBookAsync(id);

            //assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteBookAsync_DeleteFail_ReturnsBadRequest()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _bookRepMock.Setup(q => q.GetOneAsync(id)).ReturnsAsync(book);
            _bookRepMock.Setup(q => q.Delete(book)).Returns(false);

            //act
            var result = await _booksController.DeleteBookAsync(id);

            //assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task EditBookAsync_DifferentIds_ReturnsBadRequest()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _booksController.Request = new HttpRequestMessage();

            //act
            var result = await _booksController.EditBookAsync(id2, book);

            //assert
            Assert.IsInstanceOfType(result, typeof(ResponseMessageResult));
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, ((ResponseMessageResult)result).Response.StatusCode);
        }

        [TestMethod]
        public async Task EditBookAsync_ModelStateInvalid_ReturnsBadRequest()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _booksController.ModelState.AddModelError("test", "test");
            _booksController.Request = new HttpRequestMessage();

            //act
            var result = await _booksController.EditBookAsync(id, book);

            //assert
            Assert.IsInstanceOfType(result, typeof(ResponseMessageResult));
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, ((ResponseMessageResult)result).Response.StatusCode);
        }

        [TestMethod]
        public async Task EditBookAsync_BookDoesNotExist_ReturnsNotFound()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _bookRepMock.Setup(q => q.BookExistsAsync(id)).ReturnsAsync(false);

            //act
            var result = await _booksController.EditBookAsync(id, book);

            //assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task EditBookAsync_Updated_ReturnsOk()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _bookRepMock.Setup(q => q.BookExistsAsync(id)).ReturnsAsync(true);
            _bookRepMock.Setup(q => q.UpdateUntracked(book)).Returns(true);

            //act
            var result = await _booksController.EditBookAsync(id, book);

            //assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task EditBookAsync_FailedUpdate_ReturnsBadRequest()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _bookRepMock.Setup(q => q.BookExistsAsync(id)).ReturnsAsync(true);
            _bookRepMock.Setup(q => q.UpdateUntracked(book)).Returns(false);

            //act
            var result = await _booksController.EditBookAsync(id, book);

            //assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task CreateBookAsync_ModelStateInvalid_ReturnsBadRequest()
        {
            //arrange
            Book book = GetSampleBook();
            _booksController.ModelState.AddModelError("test", "test");
            _booksController.Request = new HttpRequestMessage();

            //act
            var result = await _booksController.CreateBookAsync(book);

            //assert
            Assert.IsInstanceOfType(result, typeof(ResponseMessageResult));
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, ((ResponseMessageResult)result).Response.StatusCode);
        }

        [TestMethod]
        public async Task CreateBookAsync_NoISBNAvailable_ReturnsBadRequest()
        {
            //arrange
            Book book = GetSampleBook();
            _booksController.Request = new HttpRequestMessage();
            _bookRepMock.Setup(q => q.IsISBNAvailable(book.ISBN)).Returns(false);

            //act
            var result = await _booksController.CreateBookAsync(book);

            //assert
            Assert.IsInstanceOfType(result, typeof(ResponseMessageResult));
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, ((ResponseMessageResult)result).Response.StatusCode);
        }

        [TestMethod]
        public async Task CreateBookAsync_IdAlreadyExists_ReturnsBadRequest()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _booksController.Request = new HttpRequestMessage();
            _bookRepMock.Setup(q => q.IsISBNAvailable(book.ISBN)).Returns(true);
            _bookRepMock.Setup(q => q.BookExistsAsync(book.Id)).ReturnsAsync(true);

            //act
            var result = await _booksController.CreateBookAsync(book);

            //assert
            Assert.IsInstanceOfType(result, typeof(ResponseMessageResult));
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, ((ResponseMessageResult)result).Response.StatusCode);
        }

        [TestMethod]
        public async Task CreateBookAsync_FailedToCreate_ReturnsBadRequest()
        {
            //arrange
            Book book = GetSampleBook();
            _booksController.Request = new HttpRequestMessage();
            _bookRepMock.Setup(q => q.IsISBNAvailable(book.ISBN)).Returns(true);
            _bookRepMock.Setup(q => q.BookExistsAsync(book.Id)).ReturnsAsync(false);
            _bookRepMock.Setup(q => q.Create(book)).Returns(false);

            //act
            var result = await _booksController.CreateBookAsync(book);

            //assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task CreateBookAsync_Created_ReturnsCreated()
        {
            //arrange
            string link = "https://localhost/api/books";
            Guid id = Guid.NewGuid();
            Book book = GetSampleBook();
            book.Id = id;
            _bookRepMock.Setup(q => q.IsISBNAvailable(book.ISBN)).Returns(true);
            _bookRepMock.Setup(q => q.BookExistsAsync(book.Id)).ReturnsAsync(false);
            _bookRepMock.Setup(q => q.Create(book)).Returns(true);

            _booksController.Request = new HttpRequestMessage() { RequestUri = new Uri(link) };
            _booksController.Configuration = new HttpConfiguration();
            _booksController.Configuration.Routes.MapHttpRoute(
                name: RouteConfig.RouteName,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
            _booksController.RequestContext.RouteData = new HttpRouteData(new HttpRoute(), new HttpRouteValueDictionary() { { "controller", "books" } });


            //act
            var result = await _booksController.CreateBookAsync(book);

            //assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<Book>));
            CreatedAtRouteNegotiatedContentResult<Book> createdResult = (CreatedAtRouteNegotiatedContentResult<Book>)result;
            Assert.AreEqual(book.Id, createdResult.Content.Id);
            Assert.AreEqual(RouteConfig.RouteName, createdResult.RouteName);
            Assert.AreEqual(book.Id, createdResult.RouteValues["id"]);
            Assert.AreEqual(link + "/" + id, (await createdResult.ExecuteAsync(new System.Threading.CancellationToken())).Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public async Task GetBooksAsync_Present_GetsBooks()
        {
            //arrange
            const int n = 10;
            List<Book> books = GetSampleBooks(n).ToList();
            _bookRepMock.Setup(q => q.GetAllAsync(10, 0, "", "", "")).ReturnsAsync(books);

            //act
            var result = await _booksController.GetBooksAsync();

            //assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IEnumerable<Book>>));

            List<Book> resultContent = ((OkNegotiatedContentResult<IEnumerable<Book>>)result).Content.ToList();

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(books[i], resultContent[i]);
            }
        }
    }
}
