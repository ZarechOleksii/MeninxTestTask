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

namespace Tests.DAL
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        private readonly LibraryDBContext libraryDBContext;
        private readonly Mock<ILogger<CategoryRepository>> loggerMock;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDBContext>()
                .UseInMemoryDatabase("Meninx")
                .Options;

            libraryDBContext = new LibraryDBContext(options);
            loggerMock = new Mock<ILogger<CategoryRepository>>();
        }

        [TestMethod]
        public void GetAllNoTracking_GetAllNoTracking_OrderedByNames()
        {
            //arrange
            const string name1 = "SomeName1";
            const string name2 = "SomeName2";
            const string name3 = "SomeName3";

            libraryDBContext.Categories.Add(new Category() { Name = name3, Description = "Description1" });
            libraryDBContext.Categories.Add(new Category() { Name = name2, Description = "Description2" });
            libraryDBContext.Categories.Add(new Category() { Name = name1, Description = "Description3" });
            libraryDBContext.SaveChanges();
            CategoryRepository rep = new CategoryRepository(libraryDBContext, loggerMock.Object);

            //act
            var result = rep.GetAllNoTracking();

            //assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(name1, result.ToArray()[0].Name);
            Assert.AreEqual(name2, result.ToArray()[1].Name);
            Assert.AreEqual(name3, result.ToArray()[2].Name);
        }

        [TestMethod]
        public async Task GetOneAsync_NotFound_ReturnsNull()
        {
            //arrange
            CategoryRepository rep = new CategoryRepository(libraryDBContext, loggerMock.Object);

            //act
            var result = await rep.GetOneAsync(Guid.NewGuid());

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetOneAsync_Found_ReturnsObject()
        {
            //arrange
            const string categoryName = "name";
            Guid categoryId = Guid.NewGuid();
            libraryDBContext.Categories.Add(new Category() { Id = categoryId, Name = categoryName, Description = "Description1" });
            libraryDBContext.SaveChanges();

            CategoryRepository rep = new CategoryRepository(libraryDBContext, loggerMock.Object);

            //act
            var result = await rep.GetOneAsync(categoryId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(categoryName, result.Name);
        }
    }
}
