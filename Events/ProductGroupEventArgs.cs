using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    public class ProductGroupEventArgs : EventArgs
    {
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
