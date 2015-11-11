using CatalogManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Data
{
    public interface IRepository<T> where T : BaseEntity
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
        /// <returns>Entities</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Finds entities matching condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>Entites</returns>
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Inserts entity into set
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Inserted Entity</returns>
        T Add(T entity);

        /// <summary>
        /// Removes entity from set
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Updates entity in set
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Updated Entity</returns>
        T Update(T entity);


        /// <summary>
        /// Saves changes
        /// </summary>
        void Save();
    }
}
