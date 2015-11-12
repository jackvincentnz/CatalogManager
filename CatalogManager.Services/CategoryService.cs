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

        /// <summary>
        /// Overrides generic delete function
        /// Required to handle deletion of self referencing entities
        /// </summary>
        /// <param name="entity">Category</param>
        public override void Delete(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var target = _dbset.FirstOrDefault(x => x.Id == entity.Id);
            RecursiveDelete(target);

            _context.SaveChanges();
        }
        
        /// <summary>
        /// Recursively deletes child categories
        /// </summary>
        /// <param name="parent">Category</param>
        private void RecursiveDelete(Category parent)
        {
            if (parent.Categories != null)
            {
                var categories = _dbset.Where(x => x.CategoryID == parent.Id);

                foreach (var subCat in categories)
                {
                    RecursiveDelete(subCat);
                }
            }
            _dbset.Remove(parent);
        }
    }
}
