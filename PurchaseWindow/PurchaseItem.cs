using CreditKiosk.Models;
using System;

namespace CreditKiosk.PurchaseWindow
{
    /// <summary>
    /// A single purchase item to populate List view in Purchase window.
    /// </summary>
    internal class PurchaseItem
    {
        private double amount;

        /// <summary>
        /// Amount of purchase item.
        /// </summary>
        public double Amount {
            get
            {
                return amount;
            }
            set
            {
                if (value > 0)
                {
                    amount = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Amount can not be 0 or less");
                }
            }
        }

        /// <summary>
        /// The items Product group.
        /// </summary>
        public ProductGroup ProductGroup { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="productGroup">Product group of the item.</param>
        /// <param name="amount">Amount of the item.</param>
        public PurchaseItem(ProductGroup productGroup, double amount)
        {
            ProductGroup = productGroup;
            Amount = amount;
        }
    }
}
