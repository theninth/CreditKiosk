using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CreditKiosk.Models
{
    public class Purchase : Transaction
    {
        public int ProductGroupId { get; set; }
        public ProductGroup? ProductGroup { get; set; }

        public int PersonId { get; set; }

        public Person? Person { get; set; }

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
