using System;

namespace CreditKiosk.Models
{
    public class Purchase : Transaction
    {
        public int ProductGroupId { get; set; }
        public ProductGroup? ProductGroup { get; set; }

        public override string ToString()
        {
            return $"{ProductGroup.ToString()} {Amount:n2} Kr ({Date:f})";
        }
    }
}
