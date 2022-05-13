using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreditKiosk.Managers
{
    /// <summary>
    /// Manage basic database operations for the Person table.
    /// </summary>
    class ProductGroupManager
    {
        /// <summary>
        /// Add Product type to database.
        /// </summary>
        /// <param name="productGroup">ProductGroup object to be added.</param>
        public void Add(ProductGroup productGroup)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(productGroup);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes ProductGroup from database.
        /// </summary>
        /// <param name="productGroup">ProductGroup object to be removed.</param>
        public void Remove(ProductGroup productGroup)
        {
            using (var context = new KioskDbContext())
            {
                context.ProductGroups.Remove(productGroup);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all ProductGroup objects.
        /// </summary>
        /// <returns></returns>
        public List<ProductGroup> GetAll()
        {
            using (var context = new KioskDbContext())
            {
                List<ProductGroup>? productGroups = context.ProductGroups.ToList();
                if (productGroups == null) throw new Exception("ProductGroup property should not be null if database the database is valid");
                return productGroups;
            }
        }
    }
}
