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
    public class ProductServiceTest
    {
        private IProductService _service;
        Mock<IDbContext> _mockContext;
        Mock<DbSet<Product>> _mockSet;
        IQueryable<Product> listProduct;
        private Product stubProduct;

        [TestInitialize]
        public void TestInitialize()
        {
            stubProduct = new Product { Id = 1, Name = "Test Product" };
            listProduct = new List<Product> {
                new Product {Id = 1, Name="Product A"},
                new Product {Id = 1, Name="Product B"},
                new Product {Id = 1, Name="Product C"},
                new Product {Id = 1, Name="Product D"}
             }.AsQueryable();

            _mockSet = new Mock<DbSet<Product>>();
            _mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(listProduct.Provider);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(listProduct.Expression);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(listProduct.ElementType);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(listProduct.GetEnumerator());

            _mockContext = new Mock<IDbContext>();
            _mockContext.Setup(c => c.Set<Product>()).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.Products).Returns(_mockSet.Object);

            _service = new ProductService(_mockContext.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void Product_Get_All()
        {
            //Act
            var results = _service.GetAll().ToList();

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count);
        }
        
        [TestMethod]
        public void Can_Add_Product()
        {
            //Arrange
            int Id = 1;
            Product product = new Product() { Name = "Test Product" };

            _mockSet.Setup(m => m.Add(product)).Returns((Product e) =>
            {
                e.Id = Id;
                return e;
            });
            
            //Act
            _service.Create(product);

            //Assert
            Assert.AreEqual(Id, product.Id);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Get_By_Id_Returns_Product()
        {
            //Arrange
            _mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns(stubProduct);

            //Act
            var result = _service.GetById(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Product));
        }

        [TestMethod]
        public void Can_Get_Product_By_Id()
        {
            //Arrange
            _mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns(stubProduct);

            //Act
            var result = _service.GetById(stubProduct.Id);

            //Assert
            Assert.AreSame(result, stubProduct);
        }
    }
}
