using CatalogManager.Models;
using CatalogManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatalogManager.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CatalogController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            var categories = _categoryService.GetCatalogCategories();
            var model = new CatalogViewModel
            {
                Categories = categories
            };

            return View(model);
        }
        
    }
}