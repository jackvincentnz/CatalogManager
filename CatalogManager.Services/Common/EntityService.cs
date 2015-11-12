using CatalogManager.Core;
using CatalogManager.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Services
{
    public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        protected IDbContext _context;
        protected IDbSet<T> _dbset;

        public EntityService(IDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public virtual T GetById(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return _dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        public virtual T Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var result = _dbset.Add(entity);
            _context.SaveChanges();
            return result;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbset.Remove(entity);
            _context.SaveChanges();
        }
        
        public virtual T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }
    }
}
