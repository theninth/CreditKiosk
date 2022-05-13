using CreditKiosk.Models;
using System;

/// <summary>
/// An entry of a deposit as it should be written in the log.
/// </summary>
namespace CreditKiosk.Loggers.Transaction.LogEntries
{
    internal class DepositLogEntry
    {
        private double amount;

        public DateTime Date { get; set; }
        public string Type { get; } = "Insättning";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Amount // Always returns positive number.
        {
            get { return amount; }
            set { amount = Math.Abs(value); }
        }
        // A deposit doesn't have a product group. This is to line up rows in CSV file.
        public string ProductGroup { get; } = string.Empty;  

        public string Comment { get; set; }

        public DepositLogEntry(Deposit deposit)
        {
            Date = deposit.Date;
            FirstName = deposit.Person.FirstName;
            LastName = deposit.Person.LastName;
            Amount = deposit.Amount;
            Comment = deposit.Comment;
        }
    }
}
