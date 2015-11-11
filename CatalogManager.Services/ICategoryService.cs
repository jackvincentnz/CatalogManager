using CatalogManager.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Services
{
    public interface ICategoryService : IEntityService<Category>
    {
        /// <summary>
        /// Gets top level catalog categories
        /// </summary>
        /// <returns>Categories</returns>
        List<Category> GetCatalogCategories();
    }
}
