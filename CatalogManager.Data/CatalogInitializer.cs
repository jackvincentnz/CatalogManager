using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogManager.Core.Domain;

namespace CatalogManager.Data
{
    public class CatalogInitializer : System.Data.Entity.DropCreateDatabaseAlways<CatalogManagerContext>
    {
        protected override void Seed(CatalogManagerContext context)
        {
            var categories = new List<Category>
            {
                new Category {Name="Computers, Tablets & eReaders"},
                new Category {Name="Ink, Office Supplies & Telephones"},
                new Category {Name="TV & Home Theatre"},
                new Category {Name="Cameras, Camcorders & Drones"}
            };

            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();
        }
    }
}
