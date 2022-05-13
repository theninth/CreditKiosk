using CreditKiosk.Models;
using System.Collections.Generic;
using System.Linq;

namespace CreditKiosk.PurchaseWindow.Helpers
{
    /// <summary>
    /// Helpers used in PurchaseWindow
    /// </summary>
    internal static class PurchaseHelpers
    {
        /// <summary>
        /// Takes a list of PurchaseItems and make them a list of single Purchase items with each ProductGroup summed.
        /// </summary>
        /// <param name="purchaseItems"></param>
        /// <returns>A list of Purchase items.</returns>
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
