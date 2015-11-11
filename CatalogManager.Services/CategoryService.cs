using CatalogManager.Core.Domain;
using CatalogManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Services
{
    public class CategoryService : EntityService<Category>, ICategoryService
    {
        IUnitOfWork _unitOfWork;
        ICategoryRepository _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
            : base(unitOfWork, categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Returns top level categories for catalog
        /// </summary>
        /// <returns>Categories</returns>
        public List<Category> GetCatalogCategories()
        {
            var categories = _categoryRepository.FindBy(x => x.CategoryID == null);
            return categories?.ToList();
        }
    }
}
