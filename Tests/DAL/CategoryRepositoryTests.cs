using DAL;
using DAL.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.DAL
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        private readonly LibraryDBContext _libraryDBContext;
        private readonly Mock<ILogger<CategoryRepository>> _loggerMock;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDBContext>()
                .UseInMemoryDatabase("Meninx")
                .Options;

            _libraryDBContext = new LibraryDBContext(options);
            _loggerMock = new Mock<ILogger<CategoryRepository>>();
            _categoryRepository = new CategoryRepository(_libraryDBContext, _loggerMock.Object);
        }

        [TestMethod]
        public void GetAllNoTracking_GetAllNoTracking_OrderedByNames()
        {
            //arrange
            const string name1 = "SomeName1";
            const string name2 = "SomeName2";
            const string name3 = "SomeName3";

            _libraryDBContext.Categories.Add(new Category() { Name = name3, Description = "Description1" });
            _libraryDBContext.Categories.Add(new Category() { Name = name2, Description = "Description2" });
            _libraryDBContext.Categories.Add(new Category() { Name = name1, Description = "Description3" });
            _libraryDBContext.SaveChanges();

            //act
            var result = _categoryRepository.GetAllNoTracking();

            //assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(name1, result.ToArray()[0].Name);
            Assert.AreEqual(name2, result.ToArray()[1].Name);
            Assert.AreEqual(name3, result.ToArray()[2].Name);
        }

        [TestMethod]
        public async Task GetOneAsync_NotFound_ReturnsNull()
        {
            //act
            var result = await _categoryRepository.GetOneAsync(Guid.NewGuid());

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetOneAsync_Found_ReturnsObject()
        {
            //arrange
            const string categoryName = "name";
            Guid categoryId = Guid.NewGuid();
            _libraryDBContext.Categories.Add(new Category() { Id = categoryId, Name = categoryName, Description = "Description1" });
            _libraryDBContext.SaveChanges();

            //act
            var result = await _categoryRepository.GetOneAsync(categoryId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(categoryName, result.Name);
        }
    }
}
