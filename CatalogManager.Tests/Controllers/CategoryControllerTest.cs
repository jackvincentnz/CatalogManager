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
using System.Net;

namespace CatalogManager.Tests.Controllers
{
    //todo: test for service calls that throw exceptions
    [TestClass]
    public class CategoryControllerTest
    {
        private Mock<ICategoryService> _mockCategoryService;
        private CategoryController _controller;
        private Category stubCategory = new Category
        {
            Id = 1,
                Name = "Computers, Tablets & eReaders",
                Categories = new List<Category>
                {
                    new Category { Name = "Laptops & Ultrabooks" },
                    new Category { Name = "Tablets & iPads" },
                    new Category { Name = "MacBook, iMac & Mac Pro" },
                    new Category { Name = "Desktop Computers" },
                    new Category { Name = "Monitors" }
                },
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Product A",
                        Description = "Simple test product",
                        Price = 12.50M
                    },
                    new Product
                    {
                        Name = "Product B",
                        Description = "Another Simple test product",
                        Price = 15M
                    }
                }
            };

        [TestInitialize]
        public void TestInitialize()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockCategoryService.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockCategoryService.VerifyAll();
            _controller.Dispose();
            _controller = null;
        }

        #region Delete Action Tests

        [TestMethod]
        public void Delete_Get_Action_Given_No_Id_Expect_Bad_Request()
        {
            // Act 
            var result = _controller.Delete(null) as HttpStatusCodeResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void Delete_Get_Action_Calls_CategoryService_GetById()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            //Act
            _controller.Delete(1);

            //Assert
            _mockCategoryService.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_404_If_Category_Not_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns((Category)null);

            //Act
            var result = _controller.Delete(stubCategory.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_ViewResult_If_Category_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns(stubCategory);

            //Act
            var result = _controller.Delete(stubCategory.Id) as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_DefaultView_If_Category_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns(stubCategory);

            //Act
            var result = _controller.Delete(stubCategory.Id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_Correct_ViewModel_If_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns(stubCategory);
            
            //Act
            var result = _controller.Delete(stubCategory.Id) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubCategory);
        }

        [TestMethod]
        public void Delete_Post_Action_Calls_CategoryService_Delete()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.Delete(It.IsAny<Category>())).Verifiable();

            //Act
            _controller.DeleteConfirmed(stubCategory.Id);

            //Assert
            _mockCategoryService.Verify(x => x.Delete(It.IsAny<Category>()));
        }

        [TestMethod]
        public void Delete_Post_Action_Returns_RedirectToAction()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.Delete(It.IsAny<Category>())).Verifiable();

            //Act
            var result = _controller.DeleteConfirmed(stubCategory.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Delete_Post_Action_Returns_RedirectToAction_Catalog()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.Delete(It.IsAny<Category>())).Verifiable();

            //Act
            var result = _controller.DeleteConfirmed(stubCategory.Id) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Catalog", result.RouteValues["controller"]);
        }

        #endregion

        #region Edit Action Tests

        [TestMethod]
        public void Edit_Action_Given_No_Id_Expect_Bad_Request()
        {
            // Act 
            var result = _controller.Edit((int?)null) as HttpStatusCodeResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void Edit_Get_Action_Calls_CategoryService_GetById()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();
            
            //Act
            _controller.Edit(stubCategory.Id);

            //Assert
            _mockCategoryService.Verify(x => x.GetById(It.IsAny<int>()));
        }
        
        [TestMethod]
        public void Edit_Get_Action_Returns_ViewResult_If_Category_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns(stubCategory);

            //Act
            var result = _controller.Edit(stubCategory.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Edit_Get_Action_Returns_DefaultView_If_Category_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns(stubCategory);

            //Act
            var result = _controller.Edit(stubCategory.Id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Edit_Get_Action_Returns_Correct_ViewModel_When_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns(stubCategory);

            //Act
            var result = _controller.Edit(stubCategory.Id) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubCategory);
        }

        [TestMethod]
        public void Edit_Get_Action_Returns_404_If_Category_Not_Found()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns((Category)null);

            //Act
            var result = _controller.Edit(stubCategory.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_ViewResult_If_Model_Not_Valid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Edit(stubCategory);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_DefaultView_When_Invalid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Edit(stubCategory) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_Same_ViewModel_When_Invalid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Edit(stubCategory) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubCategory);
        }

        [TestMethod]
        public void Edit_Post_Action_Calls_Correct_Methods_When_Valid()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.Update(It.IsAny<Category>())).Returns(It.IsAny<Category>());
            
            _controller.ViewData.ModelState.Clear();

            //Act
            _controller.Edit(stubCategory);

            //Assert
            _mockCategoryService.Verify(x => x.Update(It.IsAny<Category>()));            
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_RedirectToAction_When_Valid()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.Update(It.IsAny<Category>())).Returns(It.IsAny<Category>());
            _controller.ViewData.ModelState.Clear();

            //Act
            var result = _controller.Edit(stubCategory);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_RedirectToAction_Index_When_Valid()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.Update(It.IsAny<Category>())).Returns(It.IsAny<Category>());
            _controller.ViewData.ModelState.Clear();

            //Act
            var result = _controller.Edit(stubCategory) as RedirectToRouteResult;
            var routeValue = result.RouteValues["action"];

            //Assert
            Assert.AreEqual(routeValue, "Index");
        }

        #endregion

        #region Create Action Tests

        [TestMethod]
        public void Create_Get_Action_Returns_ViewResult()
        {
            //Act
            var result = _controller.Create();

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Get_Action_Returns_DefaultView()
        {
            //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Create_Post_Action_Returns_ViewResult_When_Invalid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Create(stubCategory);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Post_Action_Returns_DefaultView_When_Invalid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Create(stubCategory) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Create_Post_Action_Returns_Same_Viewmodel_When_Invalid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Create(stubCategory) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubCategory);
        }

        [TestMethod]
        public void Create_Post_Action_Calls_Correct_Methods_When_Valid()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.Create(It.IsAny<Category>())).Returns(stubCategory);
            _controller.ViewData.ModelState.Clear();

            //Act
            _controller.Create(stubCategory);

            //Assert
            _mockCategoryService.Verify(x => x.Create(It.IsAny<Category>()));
        }
        
        [TestMethod]
        public void Create_Given_Valid_Model_Expect_Redirect_To_Inserted_Category()
        {
            // Arrange
            _mockCategoryService.Setup(x => x.Create(It.IsAny<Category>())).Returns(stubCategory);

            // Act
            var result = _controller.Create(stubCategory) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Category", result.RouteValues["controller"]);
            Assert.AreEqual(stubCategory.Id, result.RouteValues["id"]);
        }

        #endregion

        #region Index Action Tests

        [TestMethod]
        public void Index_Returns_404_If_No_Category_Found()
        {
            //Arrange
            // null is returned from GetById when a category is not found
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns((Category)null);

            //Act
            var result = _controller.Index(stubCategory.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Index_Returns_View_With_CategoryModel()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubCategory);
            
            //Act
            var result = _controller.Index(stubCategory.Id) as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(Category));
        }

        [TestMethod]
        public void Index_Returns_DefaultView()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubCategory);

            //Act
            var result = _controller.Index(stubCategory.Id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Index_Returns_ViewResult()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubCategory);

            //Act
            var result = _controller.Index(stubCategory.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Index_Calls_CategoryService_GetById()
        {
            //Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            //Act
            _controller.Index(stubCategory.Id);

            //Assert
            _mockCategoryService.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Index_Returns_View_With_Same_Model_Data()
        {
            // Arrange
            _mockCategoryService.Setup(x => x.GetById(stubCategory.Id)).Returns(stubCategory);

            // Act
            ViewResult result = _controller.Index(stubCategory.Id) as ViewResult;
            var model = result.Model as Category;

            // Assert
            Assert.AreSame(stubCategory, model);
        }
        
        [TestMethod]
        public void Index_Redirect_For_No_CategoryId()
        {
            // Act
            var result = _controller.Index(null) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Catalog", result.RouteValues["controller"]);
        }

        #endregion
    }
}
