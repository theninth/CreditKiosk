using CreditKiosk.Models;
using System;

namespace CreditKiosk.Managers
{
    /// <summary>
    /// Manage basic database operations for the Transaction tables Deposit, Purchase and Credit.
    /// </summary>
    public class TransactionManager
    {
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
