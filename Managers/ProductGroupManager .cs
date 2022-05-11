using CreditKiosk.Models;
using System.Collections.Generic;
using System.Linq;

namespace CreditKiosk.Managers
{
    class ProductGroupManager
    {
        public void Add(ProductGroup productGroup)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(productGroup);
                context.SaveChanges();
            }
        }

        public void Remove(ProductGroup productGroup)
        {
            using (var context = new KioskDbContext())
            {
                context.ProductGroups.Remove(productGroup);
                context.SaveChanges();
            }
        }

        public List<ProductGroup> GetAll()
        {
            using (var context = new KioskDbContext())
            {
                return context.ProductGroups.ToList();
            }
        }
    }
}
