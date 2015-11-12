using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogManager.Core;

namespace CatalogManager.Services
{
    /// <summary>
    /// Generic Entity Service Class
    /// Can be extended and overridden to provide additional functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityService<T> : IService
        where T : BaseEntity
    {
        /// <summary>
        /// Retrieves entity by id
        /// </summary>
        /// <param name="id">Identitifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Gets all entities in set
        /// </summary>
        /// <returns>Entity</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Inserts new entity
        /// </summary>
        /// <param name="entity">The entity</param>
        T Create(T entity);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entity">The entity</param>
        T Update(T entity);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity">The entity</param>
        void Delete(T entity);
    }
}
