using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditKiosk.PurchaseWindow
{
    internal class PurchaseItem
    {
        private double amount;

        public double Amount {
            get
            {
                return amount;
            }
            set
            {
                if (value > 0)
                {
                    amount = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Amount could not be 0 or less");
                }
            }
        }

        public ProductGroup ProductGroup { get; set; }

        public PurchaseItem(ProductGroup productGroup, double amount)
        {
            ProductGroup = productGroup;
            Amount = amount;
        }
    }
}
