using CatalogManager.Core.Domain;
using CatalogManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Services
{
    public class ProductService : EntityService<Product>, IProductService
    {
        private IDbContext _context;

        public ProductService(IDbContext context)
            : base(context)
        {
            _context = context;
            _dbset = _context.Set<Product>();
        }
    }
}
