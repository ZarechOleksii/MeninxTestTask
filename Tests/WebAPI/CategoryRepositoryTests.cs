using DAL;
using DAL.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using Models;
using System;
using System.Threading.Tasks;
using WebForms.Controllers;
using DAL.Interfaces;

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
    }
}
