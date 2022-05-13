using CreditKiosk.Enums;
using CreditKiosk.Models;
using System;

namespace CreditKiosk.Managers
{
    /// <summary>
    /// Manage basic database operations for the Transaction tables Deposit, Purchase and Credit.
    /// </summary>
    public class TransactionManager
    {
        private TransactionLogger? transactionLogger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TransactionManager() { }

        /// <summary>
        /// Constructor overload for supplying a logger method.
        /// </summary>
        /// <param name="logTransaction"></param>
        public TransactionManager(TransactionLogger logTransaction) => this.transactionLogger = logTransaction;

        /// <summary>
        /// Delegate to handle logging.
        /// </summary>
        /// <param name="transaction">Transaction object.</param>
        /// <param name="transactionType">The type of transaction.</param>
        public delegate void TransactionLogger(Transaction transaction, TransactionType transactionType);

        /// <summary>
        /// Make a deposit.
        /// </summary>
        /// <param name="deposit">Deposit object.</param>
        public void Deposit(Deposit deposit)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(deposit);
                context.SaveChanges();
            }

            if (transactionLogger != null) transactionLogger(deposit, TransactionType.Deposit);
        }

        /// <summary>
        /// Make a purchase.
        /// </summary>
        /// <param name="purchase">Purchase object.</param>
        public void Purchase(Purchase purchase)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(purchase);
                context.SaveChanges();
            }

            if (transactionLogger != null) transactionLogger(purchase, TransactionType.Purchase);
        }

        /// <summary>
        /// Make a Credit.
        /// </summary>
        /// <param name="credit">Credit object.</param>
        public void Credit(Credit credit)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(credit);
                context.SaveChanges();
            }

            if (transactionLogger != null) transactionLogger(credit, TransactionType.Credit);
        }
         
        /// <summary>
        /// Overload of credit method to create the Credit object from Purchase object and an amount.
        /// </summary>
        /// <param name="purchase">Purchase to base credit on.</param>
        /// <param name="amount">Amount to credit.</param>
        public void Credit(Purchase purchase, double amount)
        {
            Credit credit = new(purchase.Id);
            credit.Amount = amount;
            credit.Comment = $"Kreditering av köp {purchase.Id}";
            Credit(credit);
        }
    }
}
