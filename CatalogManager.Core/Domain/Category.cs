using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogManager.Core.Domain
{
    public class Category : AuditableEntity<int>
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public int? CategoryID { get; set; }

        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
