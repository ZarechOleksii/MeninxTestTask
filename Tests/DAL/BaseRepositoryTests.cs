using DAL;
using DAL.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Tests.DAL
{
    [TestClass]
    public class BaseRepositoryTests
    {
        private readonly LibraryDBContext libraryDBContext;
        private readonly Mock<ILogger<BookRepository>> loggerMock;
        private readonly BookRepository _rep;

        public BaseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDBContext>()
                .UseInMemoryDatabase("Meninx")
                .Options;

            libraryDBContext = new LibraryDBContext(options);
            loggerMock = new Mock<ILogger<BookRepository>>();
            _rep = new BookRepository(libraryDBContext, loggerMock.Object);
        }

        [TestMethod]
        public void SaveChangesLogExceptionWrapper_ExceptionCaught_ReturnsFalse()
        {
            //act
            var result = _rep.SaveChangesLogExceptionWrapper(() => throw new Exception());

            //assert
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void SaveChangesLogExceptionWrapper_NoException_ReturnsTrue()
        {
            //act
            var result = _rep.SaveChangesLogExceptionWrapper(() => Console.WriteLine());

            //assert
            Assert.AreEqual(true, result);
        }
    }
}
