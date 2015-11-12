using System;

namespace CatalogManager.Core
{
    /// <summary>
    /// Base class for all auditable entities.
    /// Keep track of creation and update information
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }

        string CreatedBy { get; set; }
       
        DateTime UpdatedDate { get; set; }

        string UpdatedBy { get; set; }
    }
}
