using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditKiosk.Models
{
    public class ProductGroup
    {
        private string name;

        public int Id { get; set; }

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

        public double Total
        {
            get
            {
                using (var context = new KioskDbContext())
                {
                    return context.Purchases.Where(p => p.ProductGroupId == this.Id).Sum(i => i.Amount);
                }
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
