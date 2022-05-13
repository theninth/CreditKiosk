using CreditKiosk.Models;
using System;

namespace CreditKiosk.Loggers.Transaction.LogEntries
{
    /// <summary>
    /// An entry of a purchase as it should be written in the log.
    /// </summary>
    internal class PurchaseLogEntry
    {
        private double amount;

        public DateTime Date { get; set; }
        public string Type { get; } = "Inköp";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Amount  // Always returns negative number.
        {
            get { return amount; }
            set { amount = -Math.Abs(value); }
        }
        public string ProductGroup { get; set; }
        public string Comment { get; set; }

        public PurchaseLogEntry(Purchase purchase)
        {
            Date = purchase.Date;
            FirstName = purchase.Person.FirstName;
            LastName = purchase.Person.LastName;
            Amount = purchase.Amount;
#pragma warning disable CS8601 // Possible null reference assignment.
            ProductGroup = purchase.ProductGroup != null ? purchase.ProductGroup.Name : string.Empty;
#pragma warning restore CS8601 // Possible null reference assignment.
            Comment = purchase.Comment;
        }
    }
}
