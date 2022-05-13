using System;
using System.Linq;

namespace CreditKiosk.Models
{
    /// <summary>
    /// Class for a product group. Represents a table in Entity Framework.
    /// </summary>
    public class ProductGroup
    {
        private string name;

        /// <summary>
        /// ID of product group.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of product group.
        /// </summary>
        public string? Name {
            get { return name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Product group name can not be empty or null");
                }
                else if (value.Trim() == string.Empty)
                {
                    throw new ArgumentException("Product group name can not be blank");
                }
                else
                {
                    name = value;
                }
                
            }
        }

        /// <summary>
        /// Calculates the the total amount sold for in this Product group.
        /// 
        /// Save this to a temporary variable if used several times in a row when value is not changed
        /// in beetween to relieve the cost of calculation in DB.
        /// </summary>
        public double Total
        {
            get
            {
                using (var context = new KioskDbContext())
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    double sumOfPurchases = context.Purchases.Where(p => p.ProductGroupId == this.Id).Sum(i => i.Amount);
                    double sumOfCredits = context.Credits.Where(p => p.Purchase.ProductGroupId == this.Id).Sum(i => i.Amount);
#pragma warning restore CS8604 // Possible null reference argument.

                    return sumOfPurchases - sumOfCredits;
                }
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
