namespace CatalogManager.Core
{
    /// <summary>
    /// Base class for all indexed entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
