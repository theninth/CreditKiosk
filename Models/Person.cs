using System.Linq;

namespace CreditKiosk.Models
{
    /// <summary>
    /// Class for a person. Represents a table in Entity Framework.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// ID of person.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Firstname of person.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Lastname of person.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Calculates the the current balance of this person.
        /// 
        /// Save this to a temporary variable if used several times in a row when balance is not changed
        /// in beetween to relieve the cost of calculation in DB.
        /// </summary>
        public double Balance
        {
            get
            {
                using (var context = new KioskDbContext())
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    double depositSum = context.Deposits.Where(p => p.PersonId == this.Id).Sum(i => i.Amount);
                    double creditSum = context.Credits.Where(p => p.Purchase.Person.Id == this.Id).Sum(i => i.Amount);
                    double purchaseSum = context.Purchases.Where(p => p.PersonId == this.Id).Sum(i => i.Amount);
#pragma warning restore CS8604 // Possible null reference argument.
                    double sum = depositSum + creditSum - purchaseSum;
                    return sum;
                }
            }
        }

        public override string ToString() => $"{FirstName} {LastName}";

    }
}
