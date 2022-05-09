using System;

namespace CreditKiosk.Models
{
    public class Purchase : Transaction
    {
        public int ProductGroupId { get; set; }
        public ProductGroup? ProductGroup { get; set; }
    }
}
