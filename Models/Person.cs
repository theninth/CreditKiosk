using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditKiosk.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Balance
        {
            get
            {
                using (var context = new KioskDbContext())
                {
                    double depositSum = context.Deposits.Where(p => p.PersonId == this.Id).Sum(i => i.Amount);
                    double creditSum = context.Credits.Where(p => p.Purchase.Id == this.Id).Sum(i => i.Amount);
                    double purchaseSum = context.Purchases.Where(p => p.PersonId == this.Id).Sum(i => i.Amount);
                    double sum = depositSum + creditSum - purchaseSum;
                    return sum;
                }
            }
        }

        public override string ToString() => $"{FirstName} {LastName}";

    }
}
