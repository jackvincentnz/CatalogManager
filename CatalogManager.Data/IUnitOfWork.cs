using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Data
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits database changes
        /// </summary>
        void Commit();
    }
}
