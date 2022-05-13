using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    /// <summary>
    /// Event arguments provided for a credit.
    /// </summary>
    public class CreditEventArgs : EventArgs
    {
        public Purchase Purchase { get; set; }

        public double Amount { get; set; }

        public CreditEventArgs(Purchase? purchase, double amount)
        {
            Purchase = purchase;
            Amount = amount;
        }
    }
}
