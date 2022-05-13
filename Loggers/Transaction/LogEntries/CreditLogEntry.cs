using CreditKiosk.Models;
using System;

namespace CreditKiosk.Loggers.Transaction.LogEntries
{
    /// <summary>
    /// An entry of a deposit as it should be written in the log.
    /// </summary>
    internal class CreditLogEntry
    {
        private double amount;

        public DateTime Date { get; set; }
        public string Type { get; } = "Kreditering";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Amount  // Always returns positive number.
        {
            get { return amount; }
            set { amount = Math.Abs(value); }
        }
        public string ProductGroup { get; set; }
        public string Comment { get; set; }

        public CreditLogEntry(Credit credit)
        {
            Date = credit.Date;
            FirstName = credit.Purchase.Person.FirstName;
            LastName = credit.Purchase.Person.LastName;
            Amount = credit.Amount;
#pragma warning disable CS8601 // Possible null reference assignment.
            ProductGroup = credit.Purchase.ProductGroup != null ? credit.Purchase.ProductGroup.Name : string.Empty;
#pragma warning restore CS8601 // Possible null reference assignment.
            Comment = credit.Comment;
        }
    }
}
