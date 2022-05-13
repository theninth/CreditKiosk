using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    /// <summary>
    /// Event arguments for when a ProductGroup is needed to be supplied.
    /// </summary>
    public class ProductGroupEventArgs : EventArgs
    {
        /// <summary>
        /// Supplied ProductGroup object.
        /// </summary>
        public ProductGroup ProductGroup { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="productGroup">ProductGroup object</param>
        public ProductGroupEventArgs(ProductGroup productGroup)
        {
            this.ProductGroup = productGroup;
        }
    }
}
