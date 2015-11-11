using CatalogManager.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets top level catalog categories
        /// </summary>
        /// <returns>Categories</returns>
        List<Category> GetCatalogCategories();

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="id">Category identifier</param>
        /// <returns>Category</returns>
        Category GetById(int id);

        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns>Category</returns>
        Category Create(Category category);

        /// <summary>
        /// Updates category
        /// </summary>
        /// <param name="category">Category to update</param>
        /// <returns>Category</returns>
        Category Update(Category category);

        /// <summary>
        /// Deletes Category
        /// </summary>
        /// <param name="category">Category to delete</param>
        void Delete(Category category);
    }
}
