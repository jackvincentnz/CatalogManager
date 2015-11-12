using System;
using System.Web.Mvc;
using CatalogManager.Controllers;
using CatalogManager.Core.Domain;
using CatalogManager.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CatalogManager.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        private Mock<IProductService> _mockProductService;
        private Mock<ICategoryService> _mockCategoryService;
        private ProductController _controller;
        private readonly Product stubProduct = new Product
        {
            Id = 1,
            Name = "Product A",
            Description = "Simple test product",
            Price = 12.50M
        };

        [TestInitialize]
        public void TestInitialize()
        {
            _mockProductService = new Mock<IProductService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new ProductController(_mockProductService.Object, _mockCategoryService.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockProductService.VerifyAll();
        }

        #region Index Action Tests

        [TestMethod]
        public void Index_Redirect_To_Catalog()
        {
            // Act
            var result = _controller.Index() as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Catalog", result.RouteValues["controller"]);
        }

        #endregion

        #region Details Action Tests

        [TestMethod]
        public void Details_Redirect_For_No_ProductId()
        {
            // Act
            var result = _controller.Details(null) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Catalog", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void Details_Calls_ProductService_GetById()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            //Act
            _controller.Details(stubProduct.Id);

            //Assert
            _mockProductService.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Details_Returns_404_If_No_Product_Found()
        {
            //Arrange
            // null is returned from GetById when a product is not found
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns((Product)null);

            //Act
            var result = _controller.Details(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Details_Returns_ViewResult()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);

            //Act
            var result = _controller.Details(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Details_Returns_DefaultView()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);

            //Act
            var result = _controller.Details(stubProduct.Id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Details_Returns_View_With_CategoryModel()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);

            //Act
            var result = _controller.Details(stubProduct.Id) as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(Product));
        }

        [TestMethod]
        public void Details_Returns_View_With_Same_Model_Data()
        {
            // Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns(stubProduct);

            // Act
            ViewResult result = _controller.Details(stubProduct.Id) as ViewResult;
            var model = result.Model as Product;

            // Assert
            Assert.AreSame(stubProduct, model);
        }

        #endregion

        #region Create Action Tests
        #region Get

        [TestMethod]
        public void Create_Get_Action_Calls_CategoryService_GetById()
        {
            // Arrange
            const int expectedParentId = 1;
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            // Act
            _controller.Create(expectedParentId);

            // Assert
            _mockCategoryService.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Create_Get_Action_Returns_BadRequest_If_ParentNotExist()
        {
            // Arrange
            const int expectedParentId = 1;
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns((Category)null);

            // Act
            var result = _controller.Create(expectedParentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Create_Get_Action_Returns_ViewResult()
        {
            // Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Category());

            // Act
            var result = _controller.Create(stubProduct.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Get_Action_Returns_DefaultView()
        {
            // Arrange
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Category());

            //Act
            var result = _controller.Create(stubProduct.Id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Create_Get_Action_Returns_ProductModel()
        {
            // Arrange
            const int expectedParentId = 1;
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Category());

            // Act
            var result = _controller.Create(expectedParentId) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(Product));
        }

        [TestMethod]
        public void Create_Get_Action_Returns_ProductModel_With_ParentId()
        {
            // Arrange
            const int expectedParentId = 1;
            _mockCategoryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Category {Id = expectedParentId});

            // Act
            var result = _controller.Create(expectedParentId) as ViewResult;
            var model = result.Model as Product;

            // Assert
            Assert.AreEqual(model.CategoryId, expectedParentId);
        }

        #endregion
        #region Post

        [TestMethod]
        public void Create_Post_Action_Returns_ViewResult_When_Invalid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Create(stubProduct);

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
            var result = _controller.Create(stubProduct) as ViewResult;

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
            var result = _controller.Create(stubProduct) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubProduct);
        }

        [TestMethod]
        public void Create_Post_Action_Calls_Correct_Methods_When_Valid()
        {
            //Arrange
            _mockProductService.Setup(x => x.Create(It.IsAny<Product>())).Returns(stubProduct);
            _controller.ViewData.ModelState.Clear();

            //Act
            _controller.Create(stubProduct);

            //Assert
            _mockProductService.Verify(x => x.Create(It.IsAny<Product>()));
        }

        [TestMethod]
        public void Create_Given_Valid_Model_Expect_Redirect_To_Inserted_Category()
        {
            // Arrange
            _mockProductService.Setup(x => x.Create(It.IsAny<Product>())).Returns(stubProduct);

            // Act
            var result = _controller.Create(stubProduct) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Details", result.RouteValues["action"]);
            Assert.AreEqual("Product", result.RouteValues["controller"]);
            Assert.AreEqual(stubProduct.Id, result.RouteValues["id"]);
        }

        #endregion
        #endregion

        #region Edit Action Tests
        #region Get

        [TestMethod]
        public void Edit_Action_Given_No_Id_Expect_404()
        {
            // Act 
            var result = _controller.Edit((int?)null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Edit_Get_Action_Calls_ProductService_GetById()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            //Act
            _controller.Edit(stubProduct.Id);

            //Assert
            _mockProductService.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Edit_Get_Action_Returns_404_If_Category_Not_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns((Product)null);

            //Act
            var result = _controller.Edit(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Edit_Get_Action_Returns_ViewResult_If_Product_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns(stubProduct);

            //Act
            var result = _controller.Edit(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Edit_Get_Action_Returns_DefaultView_If_Category_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns(stubProduct);

            //Act
            var result = _controller.Edit(stubProduct.Id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Edit_Get_Action_Returns_Correct_ViewModel_When_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns(stubProduct);

            //Act
            var result = _controller.Edit(stubProduct.Id) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubProduct);
        }

        #endregion
        #region Post

        [TestMethod]
        public void Edit_Post_Action_Returns_ViewResult_If_Model_Not_Valid()
        {
            //Arrange
            _controller.ViewData.ModelState.Clear();
            _controller.ModelState.AddModelError("Code", "model is invalid");

            //Act
            var result = _controller.Edit(stubProduct);

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
            var result = _controller.Edit(stubProduct) as ViewResult;

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
            var result = _controller.Edit(stubProduct) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubProduct);
        }

        [TestMethod]
        public void Edit_Post_Action_Calls_Correct_Methods_When_Valid()
        {
            //Arrange
            _mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(stubProduct);
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);

            _controller.ViewData.ModelState.Clear();

            //Act
            _controller.Edit(stubProduct);

            //Assert
            _mockProductService.Verify(x => x.Update(It.IsAny<Product>()));
            _mockProductService.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_RedirectToAction_When_Valid()
        {
            //Arrange
            _mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(stubProduct);
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);
            _controller.ViewData.ModelState.Clear();

            //Act
            var result = _controller.Edit(stubProduct);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_404_When_No_Match()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns((Product)null);
            _controller.ViewData.ModelState.Clear();

            //Act
            var result = _controller.Edit(stubProduct);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Edit_Post_Action_Returns_RedirectToAction_Index_When_Valid_And_Matched()
        {
            //Arrange
            _mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(stubProduct);
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);
            _controller.ViewData.ModelState.Clear();

            //Act
            var result = _controller.Edit(stubProduct) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Details", result.RouteValues["action"]);
            Assert.AreEqual(stubProduct.Id, result.RouteValues["id"]);
        }

        #endregion
        #endregion

        #region Delete Action Tests

        [TestMethod]
        public void Delete_Get_Action_Given_No_Id_404()
        {
            // Act 
            var result = _controller.Delete(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Delete_Get_Action_Calls_ProductService_GetById()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            //Act
            _controller.Delete(1);

            //Assert
            _mockProductService.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_404_If_Product_Not_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns((Product)null);

            //Act
            var result = _controller.Delete(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_ViewResult_If_Product_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns(stubProduct);

            //Act
            var result = _controller.Delete(stubProduct.Id) as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_DefaultView_If_Product_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns(stubProduct);

            //Act
            var result = _controller.Delete(stubProduct.Id) as ViewResult;

            //Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Delete_Get_Action_Returns_Correct_ViewModel_If_Found()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(stubProduct.Id)).Returns(stubProduct);

            //Act
            var result = _controller.Delete(stubProduct.Id) as ViewResult;

            //Assert
            Assert.AreEqual(result.Model, stubProduct);
        }

        [TestMethod]
        public void Delete_Post_Action_Returns_404_No_Match()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns((Product)null);

            //Act
            var result = _controller.DeleteConfirmed(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Delete_Post_Action_Calls_ProductService_Delete()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);
            _mockProductService.Setup(x => x.Delete(It.IsAny<Product>())).Verifiable();

            //Act
            _controller.DeleteConfirmed(stubProduct.Id);

            //Assert
            _mockProductService.Verify(x => x.Delete(It.IsAny<Product>()));
        }

        [TestMethod]
        public void Delete_Post_Action_Returns_RedirectToAction()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);
            _mockProductService.Setup(x => x.Delete(It.IsAny<Product>())).Verifiable();

            //Act
            var result = _controller.DeleteConfirmed(stubProduct.Id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Delete_Post_Action_Returns_RedirectToAction_Category()
        {
            //Arrange
            _mockProductService.Setup(x => x.GetById(It.IsAny<int>())).Returns(stubProduct);
            _mockProductService.Setup(x => x.Delete(It.IsAny<Product>())).Verifiable();

            //Act
            var result = _controller.DeleteConfirmed(stubProduct.Id) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Details", result.RouteValues["action"]);
            Assert.AreEqual("Category", result.RouteValues["controller"]);
        }

        #endregion
    }
}
