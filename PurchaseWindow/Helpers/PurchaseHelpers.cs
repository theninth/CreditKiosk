using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditKiosk.PurchaseWindow.Helpers
{
    internal static class PurchaseHelpers
    {
        public static List<Purchase> PurchasesGroupedByProduceGroup(IEnumerable<PurchaseItem> purchaseItems)
        {
            List<Purchase> purchases = new();

            // Groups by purchaseItems by product group
            IEnumerable<IGrouping<ProductGroup, PurchaseItem>>? groups = from item in purchaseItems group item by item.ProductGroup;

            foreach (IGrouping<ProductGroup, PurchaseItem>? group in groups)
            {
                Purchase purchase = new Purchase()
                {
                    ProductGroupId = group.Key.Id,
                    Amount = group.Sum(p => p.Amount),
                };
                purchases.Add(purchase);
            }

            return purchases;
        }
    }
}
