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

        public void Credit(Credit credit)
        {
            using (var context = new KioskDbContext())
            {
                context.Add(credit);
                context.SaveChanges();
            }
        }
         
        public void Credit(Purchase purchase, double amount)
        {
            if (amount > purchase.Amount)
            {
                throw new Exception("Kan inte kreditera ett högre belopp än ursprungsköpet!");
            }

            Credit credit = new(purchase.Id);
            credit.Amount = amount;
            credit.Comment = $"Kreditering av köp {purchase.Id}";
            Credit(credit);
        }
    }
}
