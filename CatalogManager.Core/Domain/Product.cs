﻿namespace CatalogManager.Core.Domain
{
    public class Product : AuditableEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}