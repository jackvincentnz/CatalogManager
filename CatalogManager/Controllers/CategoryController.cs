using CatalogManager.Core.Domain;
using CatalogManager.Models;
using CatalogManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            Category category = _categoryService.GetById(id.Value);
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
                 var result = _categoryService.Create(category);
                return RedirectToAction("Details","Category", new { id = result.Id });
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
                // Get saved category and update name
                var toUpdate = _categoryService.GetById(category.Id);
                toUpdate.Name = category.Name;
                var result = _categoryService.Update(toUpdate);

                return RedirectToAction("Details", new { id = result.Id});
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
            _categoryService.Delete(category);
            return RedirectToAction("Index", "Catalog");
        }
    }
}