﻿using CatalogManager.Services;
using CatalogManager.Core.Domain;
using System;
using System.Web.Mvc;

namespace CatalogManager.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Catalog");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Catalog");
            }
            var product = _productService.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            var parentCategory = _categoryService.GetById(id);
            if (parentCategory == null)
            {
                return HttpNotFound();
            }
            var model = new Product {CategoryId = parentCategory.Id};

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _productService.Create(product);
                    return RedirectToAction("Details", "Category", new {id = result.CategoryId});
                }
                catch (Exception)
                {
                    //Log the error
                    ModelState.AddModelError("", "Unable to save changes. Please Try again.");
                }
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var product = _productService.GetById(id.Value);
                if (product != null)
                {
                    return View(product);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var toUpdate = _productService.GetById(product.Id);
                    if (toUpdate != null)
                    {
                        toUpdate.Name = product.Name;
                        toUpdate.Description = product.Description;
                        toUpdate.Price = product.Price;
                        var result = _productService.Update(toUpdate);

                        return RedirectToAction("Details", new {id = result.Id});
                    }
                    return HttpNotFound();
                }
                catch (Exception)
                {
                    //Log the error
                    ModelState.AddModelError("", "Unable to save changes. Please Try again.");
                }
            }
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Product product = _productService.GetById(id.Value);
                if (product != null)
                {
                    return View(product);
                }
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _productService.GetById(id);
            if (product != null)
            {
                _productService.Delete(product);
                return RedirectToAction("Details", "Category", new { id = product.CategoryId });
            }
            return HttpNotFound();
        }
    }
}