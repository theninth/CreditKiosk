using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    /// <summary>
    /// Event arguments provided for a purchase.
    /// </summary>
    public class PurchaseEventArgs : EventArgs
    {
        public Purchase Purchase { get; set; }

        public PurchaseEventArgs(Purchase purchase)
        {
            Purchase = purchase ?? throw new ArgumentNullException(nameof(purchase));
        }
    }
}
