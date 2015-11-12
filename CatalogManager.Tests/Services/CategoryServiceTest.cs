using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
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
        private ICategoryService _service;
        Mock<IDbContext> _mockContext;
        Mock<DbSet<Category>> _mockSet;
        IQueryable<Category> listCategory;
        private Category stubCategory;

        [TestInitialize]
        public void TestInitialize()
        {
            stubCategory = new Category() { Id = 1, Name = "Test Category" };
            listCategory = new List<Category>() {
                new Category {Id = 1, Name="Computers, Tablets & eReaders"},
                new Category {Id = 2, Name="Ink, Office Supplies & Telephones"},
                new Category {Id = 3, Name="TV & Home Theatre"},
                new Category {Id = 4, Name="Cameras, Camcorders & Drones"}
             }.AsQueryable();

            _mockSet = new Mock<DbSet<Category>>();
            _mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(listCategory.Provider);
            _mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(listCategory.Expression);
            _mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(listCategory.ElementType);
            _mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(listCategory.GetEnumerator());

            _mockContext = new Mock<IDbContext>();
            _mockContext.Setup(c => c.Set<Category>()).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.Categories).Returns(_mockSet.Object);

            _service = new CategoryService(_mockContext.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void Category_Get_All()
        {
            //Act
            var results = _service.GetAll().ToList();

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count);
        }
        
        [TestMethod]
        public void Can_Add_Category()
        {
            //Arrange
            int Id = 1;
            Category category = new Category() { Name = "Test Category" };

            _mockSet.Setup(m => m.Add(category)).Returns((Category e) =>
            {
                e.Id = Id;
                return e;
            });
            
            //Act
            _service.Create(category);

            //Assert
            Assert.AreEqual(Id, category.Id);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_By_Id_Returns_Category()
        {
            //Arrange
            _mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns(stubCategory);

            //Act
            var result = _service.GetById(stubCategory.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Category));
        }

        [TestMethod]
        public void Can_Get_Category_By_Id()
        {
            //Arrange
            _mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns(stubCategory);

            //Act
            var result = _service.GetById(stubCategory.Id);

            //Assert
            Assert.AreSame(result, stubCategory);
        }
    }
}
