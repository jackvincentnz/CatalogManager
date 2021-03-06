﻿using CatalogManager.Core.Domain;
using CatalogManager.Services;
using System;
using System.Net;
using System.Web.Mvc;

namespace CatalogManager.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
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
            var category = _categoryService.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        
        public ActionResult Create(int? id)
        {
            var model = new Category();
            if (id != null)
            {
                var category = _categoryService.GetById(id.Value);
                if (category != null)
                {
                    model.CategoryID = id;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,CategoryID")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _categoryService.Create(category);
                    if (result.CategoryID == null)
                    {
                        return RedirectToAction("Index", "Catalog");
                    }
                    return RedirectToAction("Details", "Category", new { id = result.CategoryID });
                }
                catch (Exception)
                {
                    //Log the error
                    ModelState.AddModelError("", "Unable to save changes. Please Try again.");
                }
            }

            return View(category);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get saved category and update name
                    var toUpdate = _categoryService.GetById(category.Id);
                    if (toUpdate != null)
                    {
                        toUpdate.Name = category.Name;
                        var result = _categoryService.Update(toUpdate);

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
            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryService.GetById(id);
            if (category != null)
            {
                _categoryService.Delete(category);
                return RedirectToAction("Index", "Catalog");
            }
            return HttpNotFound();
        }
    }
}