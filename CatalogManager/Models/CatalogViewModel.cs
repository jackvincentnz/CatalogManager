using CatalogManager.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogManager.Models
{
    public class CatalogViewModel
    {
        public CatalogViewModel()
        {
            Categories = new List<Category>();
        }

        public IList<Category> Categories { get; set; }
    }
}