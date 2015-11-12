using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogManager.Core.Domain
{
    public class Product : AuditableEntity<int>
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}