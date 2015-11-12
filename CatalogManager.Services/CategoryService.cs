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
        private IDbContext _context;

        public CategoryService(IDbContext context)
            : base(context)
        {
            _context = context;
            _dbset = _context.Set<Category>();
        }

        /// <summary>
        /// Returns top level categories for catalog
        /// </summary>
        /// <returns>Categories</returns>
        public List<Category> GetCatalogCategories()
        {
            return _dbset.Where(x => x.CategoryID == null).ToList();
        }
    }
}
