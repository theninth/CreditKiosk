using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CreditKiosk.Models
{
    /// <summary>
    /// Class for a purchase transaction. Represents a table in Entity Framework.
    /// </summary>
    public class Purchase : Transaction
    {
        /// <summary>
        /// ID of ProductGroup the purchase is connected to.
        /// </summary>
        public int ProductGroupId { get; set; }

        /// <summary>
        /// ProductGroup the purchase is connected to.
        /// </summary>
        public ProductGroup? ProductGroup { get; set; }

        /// <summary>
        /// Person ID of the person the deposit is connected to.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Person the deposit is connected to.
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// How much this purchase can be credited on (i. e. original amount minus what's already credited).
        /// </summary>
        public double? CreditableAmount
        {
            get
            {
                using (var context = new KioskDbContext())
                {
                    if (context.Credits == null) return null;  // Should not happen.

                    double creditSum = context.Credits
                        .Include("Purchase")
                        .Where(p => p.Purchase.Id == this.Id)
                        .Sum(i => i.Amount);
                    return Amount - creditSum;
                }
            }
        }

        public override string ToString()
        {
            string desc = ProductGroup != null ? ProductGroup.ToString() : string.Empty;
            return $"{desc} {Amount:n2} Kr ({Date:f})";
        }
    }
}
