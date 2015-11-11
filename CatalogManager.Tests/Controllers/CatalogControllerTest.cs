using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatalogManager;
using CatalogManager.Controllers;
using CatalogManager.Services;
using Moq;
using CatalogManager.Core.Domain;
using CatalogManager.Models;

namespace CatalogManager.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTest
    {
        private Mock<ICategoryService> _mockCategoryService;
        private CatalogController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CatalogController(_mockCategoryService.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockCategoryService.VerifyAll();
        }

        #region Index Action Tests

        [TestMethod]
        public void Index_Returns_View_With_Same_Model_Data()
        {
            // Arrange
            var stubCategories = new List<Category>
            {
                new Category {Id = 1, Name="Computers, Tablets & eReaders"},
                new Category {Id = 2, Name="Ink, Office Supplies & Telephones"},
                new Category {Id = 3, Name="TV & Home Theatre"},
                new Category {Id = 4, Name="Cameras, Camcorders & Drones"}
            };
            _mockCategoryService.Setup(x => x.GetCatalogCategories()).Returns(stubCategories);

            // Act
            ViewResult result = _controller.Index() as ViewResult;
            var model = result.Model as CatalogViewModel;

            // Assert
            Assert.AreSame(model.Categories, stubCategories);
        }

        [TestMethod]
        public void Index_Returns_View_With_CatalogViewModel()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetCatalogCategories()).Verifiable();

            //Act
            var result = _controller.Index() as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(CatalogViewModel));
        }

        [TestMethod]
        public void Index_Returns_DefaultView()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetCatalogCategories()).Verifiable();

            //Act
            var result = _controller.Index() as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Index_Calls_CategoryService_GetCatalogCategories()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetCatalogCategories()).Verifiable();

            //Act
            _controller.Index();

            //Assert
            _mockCategoryService.Verify(x => x.GetCatalogCategories());
        }

        [TestMethod]
        public void Index_Returns_ViewResult()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetCatalogCategories()).Verifiable();

            //Act
            var result = _controller.Index();

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Index()
        {
            // Act
            ViewResult result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}
