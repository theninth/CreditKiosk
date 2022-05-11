using CreditKiosk.Models;
using System;

namespace CreditKiosk.Managers
{
    public class TransactionManager
    {
        public void Deposit(Deposit deposit)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(deposit);
                context.SaveChanges();
            }
        }

        public void Purchase(Purchase purchase)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(purchase);
                context.SaveChanges();
            }
        }

        public void Credit(Purchase purchase, double amount)
        {
            Deposit creditDeposit = new Deposit();

            if (amount > purchase.Amount)
            {
                throw new Exception("Kan inte kreditera ett högre belopp än urspringsköpet!");
            }

            creditDeposit.PersonId = purchase.PersonId;
            creditDeposit.Amount = amount;
            creditDeposit.Comment = $"Kreditering av köp {purchase.Id}";

            Deposit(creditDeposit);
        }
    }
}
