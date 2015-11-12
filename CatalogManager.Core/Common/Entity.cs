namespace CatalogManager.Core
{
    /// <summary>
    /// Base class for all Entities. 
    /// Required for generic services
    /// </summary>
    public abstract class  BaseEntity
    {
    }

    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        public virtual T Id { get; set; }
    }
}
