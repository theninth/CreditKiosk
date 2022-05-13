using CreditKiosk.Models;
using System.Windows.Controls;

namespace CreditKiosk.PurchaseWindow
{
    /// <summary>
    /// A custom button that also can keep track of a ProductGroup via it's ConnectedProductGroup property.
    /// </summary>
    internal class ProductGroupButton : Button
    {
        /// <summary>
        /// ProductGroup to keep track of.
        /// </summary>
        public ProductGroup? ConnectedProductGroup { get; set; }
    }
}
