using System.Collections.Generic;

namespace CatalogManager.Core.Domain
{
    public class Category : AuditableEntity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public int? CategoryID { get; set; }

        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
