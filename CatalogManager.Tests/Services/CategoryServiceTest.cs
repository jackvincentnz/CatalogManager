using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CatalogManager.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatalogManager.Services;
using CatalogManager.Data;
using Moq;

namespace CatalogManager.Tests.Services
{
    [TestClass]
    public class CategoryServiceTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private ICategoryService _categoryService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockUnitOfWork.Object, _mockCategoryRepository.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockCategoryRepository.VerifyAll();
            _mockUnitOfWork.VerifyAll();
        }

        #region GetCatalogCategories Method

        [TestMethod]
        public void GetCatalogCategories_Calls_Respository_FindBy()
        {
            // Arrange
            _mockCategoryRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Category, bool>>>())).Verifiable();

            // Act
            _categoryService.GetCatalogCategories();

            // Assert
            _mockCategoryRepository.Verify(x => x.FindBy(It.IsAny<Expression<Func<Category, bool>>>()));
        }

        [TestMethod]
        public void GetCatalogCategories_Returns_Null_If_Empty()
        {
            // Arrange
            _mockCategoryRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Category, bool>>>()))
                .Returns((IEnumerable<Category>)null);

            // Act
            var result = _categoryService.GetCatalogCategories();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCatalogCategories_Returns_List()
        {
            // Arrange
            var stubCategories = new List<Category>
            {
                new Category {Id = 1, Name="Computers, Tablets & eReaders"},
                new Category {Id = 2, Name="Ink, Office Supplies & Telephones"},
                new Category {Id = 3, Name="TV & Home Theatre"},
                new Category {Id = 4, Name="Cameras, Camcorders & Drones"}
            }.AsEnumerable();

            _mockCategoryRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Category, bool>>>()))
                .Returns(stubCategories);

            // Act
            var result = _categoryService.GetCatalogCategories();

            // Assert
            Assert.IsInstanceOfType(result, typeof(List<Category>));
        }

        #endregion
    }
}
